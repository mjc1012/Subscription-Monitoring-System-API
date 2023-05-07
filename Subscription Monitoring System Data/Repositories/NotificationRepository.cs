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
    public class NotificationRepository : INotificationRepository
    {
        private readonly DataContext _context;
        public NotificationRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Notification?> Get(int id)
        {
            try
            {
                return await _context.Notifications!.Where(p => p.Id == id).Include(p => p.UserNotifications).ThenInclude(p => p.User).Include(p => p.Subscription).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Create(Notification notification, List<int> userIds)
        {
            try
            {
                foreach (int id in userIds)
                {
                    notification.UserNotifications.Add(new UserNotification()
                    {
                        UserId = id,
                        Notification = notification,
                        IsActive = true
                    });
                }
                await _context.Notifications!.AddAsync(notification);
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
                Notification? notification = await Get(id);
                _context.Notifications!.Remove(notification!);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Notification>> GetList(List<int> ids)
        {
            try
            {
                return await _context.Notifications!.Where(p => ids.Contains(p.Id)).Include(p => p.UserNotifications).ThenInclude(p => p.User).Include(p => p.Subscription).ToListAsync();
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
                List<Notification> notifications = await GetList(ids);
                _context.Notifications!.RemoveRange(notifications);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
