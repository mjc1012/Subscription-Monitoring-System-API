using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Data.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly DataContext _context;
        public SubscriptionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Subscription?> GetActive(int id)
        {
            try
            {
                return await _context.Subscriptions!.Where(p => p.Id == id && p.IsActive).Include(p => p.UpdatedBy).Include(p => p.CreatedBy).Include(p => p.Client).Include(p => p.Service).ThenInclude(p => p.ServiceType).Include(p => p.SubscriptionHistory).Include(p => p.SubscriptionHistories).Include(p => p.Notifications).Include(p => p.SubscriptionUsers).ThenInclude(p => p.User).Include(p => p.SubscriptionClients).ThenInclude(p => p.Client).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Subscription?> GetInactive(int id)
        {
            try
            {
                return await _context.Subscriptions!.Where(p => p.Id == id && !p.IsActive).Include(p => p.UpdatedBy).Include(p => p.CreatedBy).Include(p => p.Client).Include(p => p.Service).ThenInclude(p => p.ServiceType).Include(p => p.SubscriptionHistory).Include(p => p.SubscriptionHistories).Include(p => p.Notifications).Include(p => p.SubscriptionUsers).ThenInclude(p => p.User).Include(p => p.SubscriptionClients).ThenInclude(p => p.Client).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Subscription>> GetList(List<int> ids)
        {
            try
            {
                return await _context.Subscriptions!.Where(p => ids.Contains(p.Id)).Include(p => p.UpdatedBy).Include(p => p.CreatedBy).Include(p => p.Client).Include(p => p.Service).ThenInclude(p => p.ServiceType).Include(p => p.SubscriptionHistory).Include(p => p.SubscriptionHistories).Include(p => p.Notifications).Include(p => p.SubscriptionUsers).ThenInclude(p => p.User).Include(p => p.SubscriptionClients).ThenInclude(p => p.Client).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Subscription>> GetHistoryList(int id)
        {
            try
            {
                return await _context.Subscriptions!.Where(p => p.SubscriptionHistoryId == id).Include(p => p.UpdatedBy).Include(p => p.CreatedBy).Include(p => p.Client).Include(p => p.Service).ThenInclude(p => p.ServiceType).Include(p => p.SubscriptionHistory).Include(p => p.SubscriptionHistories).Include(p => p.Notifications).Include(p => p.SubscriptionUsers).ThenInclude(p => p.User).Include(p => p.SubscriptionClients).ThenInclude(p => p.Client).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Subscription>> GetList()
        {
            try
            {
                return await _context.Subscriptions!.Include(p => p.UpdatedBy).Include(p => p.CreatedBy).Include(p => p.Client).Include(p => p.Service).ThenInclude(p => p.ServiceType).Include(p => p.SubscriptionHistory).Include(p => p.SubscriptionHistories).Include(p => p.Notifications).Include(p => p.SubscriptionUsers).ThenInclude(p => p.User).Include(p => p.SubscriptionClients).ThenInclude(p => p.Client).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Subscription> Create(Subscription subscription, List<int> clientIds, List<int> userIds)
        {
            try
            {
                subscription.IsActive = true;
                foreach(int id in clientIds)
                {
                    subscription.SubscriptionClients.Add(new SubscriptionClient()
                    {
                        ClientId = id,
                        Subscription = subscription
                    });
                }
                foreach (int id in userIds)
                {
                    subscription.SubscriptionUsers.Add(new SubscriptionUser()
                    {
                        UserId = id,
                        Subscription = subscription
                    });
                }
                await _context.Subscriptions!.AddAsync(subscription);
                await _context.SaveChangesAsync();
                return subscription;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Subscription> CreateHistory(int id)
        {
            try
            {
                Subscription? subscription = await GetActive(id);
                Subscription subscriptionHistory = new()
                {
                    StartDate = subscription!.StartDate,
                    EndDate = subscription.EndDate,
                    IsActive = false,
                    IsExpired = subscription.IsExpired,
                    CreatedOn = subscription.CreatedOn,
                    UpdatedOn = subscription.UpdatedOn,
                    CreatedById = subscription.CreatedById,
                    UpdatedById = subscription.UpdatedById,
                    ClientId = subscription.ClientId,
                    ServiceId = subscription.ServiceId,
                    SubscriptionHistoryId = id,
                    Notifications = subscription.Notifications
                };

                List<int> clientIds = subscription.SubscriptionClients.Select(p => p.ClientId).ToList();
                foreach (var clientId in clientIds)
                {
                    subscriptionHistory.SubscriptionClients.Add(new SubscriptionClient()
                    {
                        ClientId = clientId,
                        Subscription = subscriptionHistory
                    });
                }

                List<int> userIds = subscription.SubscriptionUsers.Select(p => p.UserId).ToList();
                foreach (int userId in userIds)
                {
                    subscriptionHistory.SubscriptionUsers.Add(new SubscriptionUser()
                    {
                        UserId = userId,
                        Subscription = subscriptionHistory
                    });
                }
                await _context.Subscriptions!.AddAsync(subscriptionHistory);
                await _context.SaveChangesAsync();
                return subscriptionHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Subscription> Update(Subscription subscription, List<int> clientIds, List<int> userIds)
        {
            try
            {
                Subscription? subscriptionUpdate = await GetActive(subscription.Id);
                subscriptionUpdate!.StartDate = subscription.StartDate;
                subscriptionUpdate.EndDate = subscription.EndDate;
                subscriptionUpdate.ClientId = subscription.ClientId;
                subscriptionUpdate.ServiceId = subscription.ServiceId;
                subscriptionUpdate.UpdatedOn = subscription.UpdatedOn;
                subscriptionUpdate.UpdatedById = subscription.UpdatedById;

                List<SubscriptionClient> newClients= new();
                foreach (int id in clientIds)
                {
                    newClients.Add(new SubscriptionClient()
                    {
                        ClientId = id,
                        SubscriptionId = subscriptionUpdate.Id
                    });
                }
                subscriptionUpdate.SubscriptionClients = newClients;

                List<SubscriptionUser> newUsers = new();
                foreach (int id in userIds)
                {
                    newUsers.Add(new SubscriptionUser()
                    {
                        UserId = id,
                        SubscriptionId = subscriptionUpdate.Id
                    });
                }
                subscriptionUpdate.SubscriptionUsers = newUsers;

                _context.Subscriptions!.Update(subscriptionUpdate);
                await _context.SaveChangesAsync();
                return subscriptionUpdate;
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
                List<Subscription> subscriptionHistory = await GetHistoryList(id);
                Subscription? subscription = await GetInactive(id);
                subscriptionHistory.Add(subscription!);
                _context.Subscriptions!.RemoveRange(subscriptionHistory);
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
                Subscription? subscription = await GetActive(id);
                subscription!.IsActive = false;
                _context.Subscriptions!.Update(subscription);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Expired(int id)
        {
            try
            {
                Subscription? subscription = await GetActive(id);
                subscription!.IsExpired = true;
                _context.Subscriptions!.Update(subscription);
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
                List<Subscription> subscriptions = await GetList(ids);
                foreach (int id in ids)
                {
                    subscriptions.AddRange(await GetHistoryList(id));
                }
                _context.Subscriptions!.RemoveRange(subscriptions);
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
                List<Subscription> subscriptions = await GetList(ids);
                subscriptions.ForEach(p => p.IsActive = false);
                _context.Subscriptions!.UpdateRange(subscriptions);
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
                Subscription? subscription = await GetInactive(id);
                subscription!.IsActive = true;
                _context.Subscriptions!.Update(subscription);
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
                List<Subscription> subscriptions = await GetList(ids);
                subscriptions.ForEach(p => p.IsActive = true);
                _context.Subscriptions!.UpdateRange(subscriptions);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> SubscriptionExists(Subscription subscription)
        {
            try
            {
                return await _context.Subscriptions!.AnyAsync(p => p.ServiceId == subscription.ServiceId && p.ClientId == subscription.ClientId && p.Id != subscription.Id && p.IsActive);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
