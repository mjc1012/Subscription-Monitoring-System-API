using Microsoft.EntityFrameworkCore;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetActiveList()
        {
            try
            {
                return await _context.Users.Where(p => p.IsActive == true).Include(p => p.CreatedSubscriptions).Include(p => p.UpdatedSubscriptions).Include(p => p.SubscriptionUsers).ThenInclude(p => p.Subscription).Include(p => p.UserNotifications).ThenInclude(p => p.Notification).Include(p => p.Department).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetActive(int id)
        {
            try
            {
                return await _context.Users.Where(p => p.Id == id && p.IsActive == true).Include(p => p.CreatedSubscriptions).Include(p => p.UpdatedSubscriptions).Include(p => p.SubscriptionUsers).ThenInclude(p => p.Subscription).Include(p => p.UserNotifications).ThenInclude(p => p.Notification).Include(p => p.Department).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetInactive(int id)
        {
            try
            {
                return await _context.Users.Where(p => p.Id == id && p.IsActive == false).Include(p => p.CreatedSubscriptions).Include(p => p.UpdatedSubscriptions).Include(p => p.SubscriptionUsers).ThenInclude(p => p.Subscription).Include(p => p.UserNotifications).ThenInclude(p => p.Notification).Include(p => p.Department).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetActive(string code)
        {
            try
            {
                return await _context.Users.Where(p => p.Code == code && p.IsActive == true).Include(p => p.CreatedSubscriptions).Include(p => p.UpdatedSubscriptions).Include(p => p.SubscriptionUsers).ThenInclude(p => p.Subscription).Include(p => p.UserNotifications).ThenInclude(p => p.Notification).Include(p => p.Department).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetActiveByEmail(string emailAddress)
        {
            try
            {
                return await _context.Users.Where(p => p.EmailAddress == emailAddress && p.IsActive == true).Include(p => p.CreatedSubscriptions).Include(p => p.UpdatedSubscriptions).Include(p => p.SubscriptionUsers).ThenInclude(p => p.Subscription).Include(p => p.UserNotifications).ThenInclude(p => p.Notification).Include(p => p.Department).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<User>> GetList(List<int> ids)
        {
            try
            {
                return await _context.Users.Where(p => ids.Contains(p.Id)).Include(p => p.CreatedSubscriptions).Include(p => p.UpdatedSubscriptions).Include(p => p.SubscriptionUsers).ThenInclude(p => p.Subscription).Include(p => p.UserNotifications).ThenInclude(p => p.Notification).Include(p => p.Department).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<User>> GetList(UserFilterDto filter)
        {
            try
            {
                if (filter.Page == 0) filter.Page = 1;

                List<User> users = await _context.Users.Where(p => (filter.Id == 0 || p.Id == filter.Id) && 
                (string.IsNullOrEmpty(filter.Code) || p.Code == filter.Code) &&
                (string.IsNullOrEmpty(filter.FirstName) || p.FirstName == filter.FirstName) && 
                (string.IsNullOrEmpty(filter.MiddleName) || p.MiddleName == filter.MiddleName) &&
                (string.IsNullOrEmpty(filter.LastName) || p.LastName == filter.LastName) &&
                (string.IsNullOrEmpty(filter.EmailAddress) || p.EmailAddress == filter.EmailAddress) &&
                (string.IsNullOrEmpty(filter.DepartmentName) || p.Department.Name == filter.DepartmentName) && p.IsActive == filter.IsActive)
                    .Include(p => p.CreatedSubscriptions).Include(p => p.UpdatedSubscriptions).Include(p => p.SubscriptionUsers).ThenInclude(p => p.Subscription).Include(p => p.UserNotifications).ThenInclude(p => p.Notification).Include(p => p.Department).ToListAsync();

                return (!string.IsNullOrEmpty(filter.SortOrder) && filter.SortOrder.Equals(SortDirectionConstants.Descending)) ? SortDescending(filter.SortBy, users) : SortAscending(filter.SortBy, users);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<User> SortAscending(string sortBy, List<User> users)
        {
            return sortBy switch
            {
                UserConstants.HeaderId => users.OrderBy(p => p.Id).ToList(),
                UserConstants.HeaderCode => users.OrderBy(p => p.Code).ToList(),
                UserConstants.HeaderFirstName => users.OrderBy(p => p.FirstName).ToList(),
                UserConstants.HeaderMiddleName => users.OrderBy(p => p.MiddleName).ToList(),
                UserConstants.HeaderLastName => users.OrderBy(p => p.LastName).ToList(),
                UserConstants.HeaderEmailAddress => users.OrderBy(p => p.EmailAddress).ToList(),
                UserConstants.HeaderDepartmentName => users.OrderBy(p => p.Department.Name).ToList(),
                _ => users.OrderBy(p => p.Id).ToList(),
            };
        }

        public List<User> SortDescending(string sortBy, List<User> users)
        {
            return sortBy switch
            {
                UserConstants.HeaderId => users.OrderByDescending(p => p.Id).ToList(),
                UserConstants.HeaderCode => users.OrderByDescending(p => p.Code).ToList(),
                UserConstants.HeaderFirstName => users.OrderByDescending(p => p.FirstName).ToList(),
                UserConstants.HeaderMiddleName => users.OrderByDescending(p => p.MiddleName).ToList(),
                UserConstants.HeaderLastName => users.OrderByDescending(p => p.LastName).ToList(),
                UserConstants.HeaderEmailAddress => users.OrderByDescending(p => p.EmailAddress).ToList(),
                UserConstants.HeaderDepartmentName => users.OrderByDescending(p => p.Department.Name).ToList(),
                _ => users.OrderByDescending(p => p.Id).ToList(),
            };
        }

        public async Task Create(User user)
        {
            try
            {
                user.IsActive = true;
                user.Password = PasswordHasher.HashPassword(PasswordHasher.CreateTemporaryPassword(user.LastName)); 
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveTokens(User user)
        {
            try
            {
                var userUpdate = await GetActive(user.Code);
                userUpdate.ResetPasswordToken = user.ResetPasswordToken;
                userUpdate.ResetPasswordExpiry = user.ResetPasswordExpiry;
                userUpdate.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;
                userUpdate.AccessToken = user.AccessToken;
                userUpdate.RefreshToken = user.RefreshToken;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ChangePassword(User user)
        {
            try
            {
                var userUpdate = await GetActive(user.Code);
                userUpdate.Password = user.Password;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(User user)
        {
            try
            {
                var userUpdate = await GetActive(user.Id);
                if (!string.IsNullOrEmpty(user.ProfilePictureImageName)) userUpdate.ProfilePictureImageName = user.ProfilePictureImageName;
                userUpdate.Code = user.Code;
                userUpdate.FirstName = user.FirstName;
                userUpdate.MiddleName = user.MiddleName;
                userUpdate.LastName = user.LastName;
                userUpdate.EmailAddress = user.EmailAddress;
                userUpdate.DepartmentId = user.DepartmentId;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task HardDelete(int id)
        {
            try
            {
                User user = await GetInactive(id);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SoftDelete(int id)
        {
            try
            {
                User user = await GetActive(id);
                user.IsActive = false;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task HardDelete(List<int> ids)
        {
            try
            {
                List<User> users = await GetList(ids);
                _context.Users.RemoveRange(users);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SoftDelete(List<int> ids)
        {
            try
            {
                List<User> users = await GetList(ids);
                users.ForEach(p => p.IsActive = false);
                _context.Users.UpdateRange(users);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Restore(int id)
        {
            try
            {
                User user = await GetInactive(id);
                user.IsActive = true;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Restore(List<int> ids)
        {
            try
            {
                List<User> users = await GetList(ids);
                users.ForEach(p => p.IsActive = true);
                _context.Users.UpdateRange(users);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UserExists(User user)
        {
            try
            {
                return await _context.Users.AnyAsync(p => p.Code == user.Code && p.Id != user.Id && p.IsActive);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RefreshTokenExists(string refreshToken)
        {
            try
            {
                return await _context.Users.AnyAsync(p => p.RefreshToken == refreshToken && p.IsActive);
            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }
}
