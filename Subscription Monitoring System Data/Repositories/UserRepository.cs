using Microsoft.EntityFrameworkCore;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
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
                return await _context.Users!.Where(p => p.IsActive).Include(p => p.CreatedSubscriptions).Include(p => p.UpdatedSubscriptions).Include(p => p.SubscriptionUsers).ThenInclude(p => p.Subscription).Include(p => p.UserNotifications).ThenInclude(p => p.Notification).Include(p => p.Department).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User?> GetActive(int id)
        {
            try
            {
                return await _context.Users!.Where(p => p.Id == id && p.IsActive).Include(p => p.CreatedSubscriptions).Include(p => p.UpdatedSubscriptions).Include(p => p.SubscriptionUsers).ThenInclude(p => p.Subscription).Include(p => p.UserNotifications).ThenInclude(p => p.Notification).Include(p => p.Department).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User?> GetInactive(int id)
        {
            try
            {
                return await _context.Users!.Where(p => p.Id == id && !p.IsActive).Include(p => p.CreatedSubscriptions).Include(p => p.UpdatedSubscriptions).Include(p => p.SubscriptionUsers).ThenInclude(p => p.Subscription).Include(p => p.UserNotifications).ThenInclude(p => p.Notification).Include(p => p.Department).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User?> GetActive(string code)
        {
            try
            {
                return await _context.Users!.Where(p => p.Code == code && p.IsActive).Include(p => p.CreatedSubscriptions).Include(p => p.UpdatedSubscriptions).Include(p => p.SubscriptionUsers).ThenInclude(p => p.Subscription).Include(p => p.UserNotifications).ThenInclude(p => p.Notification).Include(p => p.Department).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User?> GetActiveByEmail(string emailAddress)
        {
            try
            {
                return await _context.Users!.Where(p => p.EmailAddress == emailAddress && p.IsActive).Include(p => p.CreatedSubscriptions).Include(p => p.UpdatedSubscriptions).Include(p => p.SubscriptionUsers).ThenInclude(p => p.Subscription).Include(p => p.UserNotifications).ThenInclude(p => p.Notification).Include(p => p.Department).FirstOrDefaultAsync();
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
                return await _context.Users!.Where(p => ids.Contains(p.Id)).Include(p => p.CreatedSubscriptions).Include(p => p.UpdatedSubscriptions).Include(p => p.SubscriptionUsers).ThenInclude(p => p.Subscription).Include(p => p.UserNotifications).ThenInclude(p => p.Notification).Include(p => p.Department).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<User>> GetList()
        {
            try
            {

                return await _context.Users!.Include(p => p.CreatedSubscriptions).Include(p => p.UpdatedSubscriptions).Include(p => p.SubscriptionUsers).ThenInclude(p => p.Subscription).Include(p => p.UserNotifications).ThenInclude(p => p.Notification).Include(p => p.Department).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Create(User user)
        {
            try
            {
                user.IsActive = true;
                user.Password = PasswordHasher.HashPassword(PasswordConstants.CreateTemporaryPassword(user.LastName)); 
                await _context.Users!.AddAsync(user);
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
                User? userUpdate = await GetActive(user.Code);
                userUpdate!.ResetPasswordToken = user.ResetPasswordToken;
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
                User? userUpdate = await GetActive(user.Code);
                userUpdate!.Password = user.Password;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(User newUser, User oldUser)
        {
            try
            {
                User? userUpdate = await GetActive(newUser.Id);
                if (!string.IsNullOrEmpty(newUser.ProfilePictureImageName)) userUpdate!.ProfilePictureImageName = newUser.ProfilePictureImageName;
                userUpdate!.Code = newUser.Code;
                userUpdate.FirstName = newUser.FirstName;
                userUpdate.MiddleName = newUser.MiddleName;
                userUpdate.LastName = newUser.LastName;
                userUpdate.EmailAddress = newUser.EmailAddress;
                userUpdate.DepartmentId = newUser.DepartmentId;
                if (newUser.EmailAddress != oldUser.EmailAddress) userUpdate.Password = PasswordHasher.HashPassword(PasswordConstants.CreateTemporaryPassword(userUpdate.LastName));
                _context.Users!.Update(userUpdate);
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
                User? user = await GetInactive(id);
                _context.Users!.Remove(user!);
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
                User? user = await GetActive(id);
                user!.IsActive = false;
                _context.Users!.Update(user);
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
                _context.Users!.RemoveRange(users);
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
                _context.Users!.UpdateRange(users);
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
                User? user = await GetInactive(id);
                user!.IsActive = true;
                _context.Users!.Update(user);
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
                _context.Users!.UpdateRange(users);
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
                return await _context.Users!.AnyAsync(p => (p.Code == user.Code || p.EmailAddress == user.EmailAddress) && p.Id != user.Id && p.IsActive);
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
                return await _context.Users!.AnyAsync(p => p.RefreshToken == refreshToken && p.IsActive);
            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }
}
