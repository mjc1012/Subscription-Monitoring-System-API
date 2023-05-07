using Microsoft.EntityFrameworkCore;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Repositories
{
    public class UserNotificationRepository : IUserNotificationRepository
    {
        private readonly DataContext _context;
        public UserNotificationRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<UserNotification?> GetActive(int id)
        {
            try
            {
                return await _context.UserNotifications!.Where(p => p.Id == id && p.IsActive).Include(p => p.Notification).Include(p => p.User).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserNotification?> GetInactive(int id)
        {
            try
            {
                return await _context.UserNotifications!.Where(p => p.Id == id && !p.IsActive).Include(p => p.Notification).Include(p => p.User).FirstOrDefaultAsync();
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
                UserNotification? notification = await GetInactive(id);
                _context.UserNotifications!.Remove(notification!);
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
                UserNotification? notification = await GetActive(id);
                notification!.IsActive = false;
                _context.UserNotifications!.Update(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserNotification>> GetList(List<int> ids)
        {
            try
            {
                return await _context.UserNotifications!.Where(p => ids.Contains(p.Id)).Include(p => p.Notification).Include(p => p.User).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserNotification>> GetList(int userId)
        {
            try
            {
                List<UserNotification> userNotifications = await _context.UserNotifications!.Where(p => p.UserId == userId && p.IsActive).Include(p => p.Notification).Include(p => p.User).ToListAsync();
                userNotifications.ForEach(p => p.IsSeen = true);
                _context.UserNotifications!.UpdateRange(userNotifications);
                await _context.SaveChangesAsync();
                return userNotifications;

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
                List<UserNotification> notifications = await GetList(ids);
                _context.UserNotifications!.RemoveRange(notifications);
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
                List<UserNotification> notifications = await GetList(ids);
                notifications.ForEach(p => p.IsActive = false);
                _context.UserNotifications!.UpdateRange(notifications);
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
                UserNotification? notification = await GetInactive(id);
                notification!.IsActive = true;
                _context.UserNotifications!.Update(notification);
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
                List<UserNotification> notifications = await GetList(ids);
                notifications.ForEach(p => p.IsActive = true);
                _context.UserNotifications!.UpdateRange(notifications);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
