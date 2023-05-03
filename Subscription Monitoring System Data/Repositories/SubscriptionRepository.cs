using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
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
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly DataContext _context;
        public SubscriptionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Subscription> GetActive(int id)
        {
            try
            {
                return await _context.Subscriptions.Where(p => p.Id == id && p.IsActive == true).Include(p => p.UpdatedBy).Include(p => p.CreatedBy).Include(p => p.Client).Include(p => p.Service).Include(p => p.SubscriptionHistory).Include(p => p.SubscriptionHistories).Include(p => p.Notifications).Include(p => p.SubscriptionUsers).ThenInclude(p => p.User).Include(p => p.SubscriptionClients).ThenInclude(p => p.Client).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Subscription> GetInactive(int id)
        {
            try
            {
                return await _context.Subscriptions.Where(p => p.Id == id && p.IsActive == false).Include(p => p.UpdatedBy).Include(p => p.CreatedBy).Include(p => p.Client).Include(p => p.Service).Include(p => p.SubscriptionHistory).Include(p => p.SubscriptionHistories).Include(p => p.Notifications).Include(p => p.SubscriptionUsers).ThenInclude(p => p.User).Include(p => p.SubscriptionClients).ThenInclude(p => p.Client).FirstOrDefaultAsync();
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
                return await _context.Subscriptions.Where(p => ids.Contains(p.Id)).Include(p => p.UpdatedBy).Include(p => p.CreatedBy).Include(p => p.Client).Include(p => p.Service).Include(p => p.SubscriptionHistory).Include(p => p.SubscriptionHistories).Include(p => p.Notifications).Include(p => p.SubscriptionUsers).ThenInclude(p => p.User).Include(p => p.SubscriptionClients).ThenInclude(p => p.Client).ToListAsync();
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
                return await _context.Subscriptions.Where(p => p.SubscriptionHistoryId == id).Include(p => p.UpdatedBy).Include(p => p.CreatedBy).Include(p => p.Client).Include(p => p.Service).Include(p => p.SubscriptionHistory).Include(p => p.SubscriptionHistories).Include(p => p.Notifications).Include(p => p.SubscriptionUsers).ThenInclude(p => p.User).Include(p => p.SubscriptionClients).ThenInclude(p => p.Client).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Subscription>> GetList(SubscriptionFilterDto filter)
        {
            try
            {
                if (filter.Page == 0) filter.Page = 1;

                List<Subscription> subscriptions = await _context.Subscriptions.Where(p => (filter.Id == 0 || p.Id == filter.Id) &&
                (string.IsNullOrEmpty(filter.StartDate) || p.StartDate.ToString("yyyy-MM-dd HH:mm:ss") == filter.StartDate) &&
                (string.IsNullOrEmpty(filter.EndDate) || p.EndDate.ToString("yyyy-MM-dd HH:mm:ss") == filter.EndDate) &&
                (filter.TotalPrice == 0 || p.TotalPrice == filter.TotalPrice) &&
                (string.IsNullOrEmpty(filter.ClientName) || p.Client.Name == filter.ClientName) &&
                (string.IsNullOrEmpty(filter.ServiceName) || p.Service.Name == filter.ServiceName) &&
                (string.IsNullOrEmpty(filter.CreatedByCode) || p.CreatedBy.Code == filter.CreatedByCode) &&
                (string.IsNullOrEmpty(filter.UpdatedByCode) || p.UpdatedBy.Code == filter.UpdatedByCode) && p.IsActive == filter.IsActive
                && p.IsExpired == filter.IsExpired).Include(p => p.UpdatedBy).Include(p => p.CreatedBy).Include(p => p.Client).Include(p => p.Service)
                    .Include(p => p.SubscriptionHistory).Include(p => p.SubscriptionHistories).Include(p => p.Notifications).Include(p => p.SubscriptionUsers).ThenInclude(p => p.User).Include(p => p.SubscriptionClients).ThenInclude(p => p.Client).ToListAsync();

                return (!string.IsNullOrEmpty(filter.SortOrder) && filter.SortOrder.Equals(SortDirectionConstants.Descending)) ? SortDescending(filter.SortBy, subscriptions) : SortAscending(filter.SortBy, subscriptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Subscription> SortAscending(string sortBy, List<Subscription> subscriptions)
        {
            return sortBy switch
            {
                SubscriptionConstants.HeaderId => subscriptions.OrderBy(p => p.Id).ToList(),
                SubscriptionConstants.HeaderStartDate => subscriptions.OrderBy(p => p.StartDate).ToList(),
                SubscriptionConstants.HeaderEndDate => subscriptions.OrderBy(p => p.EndDate).ToList(),
                SubscriptionConstants.HeaderTotalPrice => subscriptions.OrderBy(p => p.TotalPrice).ToList(),
                SubscriptionConstants.HeaderClientName => subscriptions.OrderBy(p => p.Client.Name).ToList(),
                SubscriptionConstants.HeaderServiceName => subscriptions.OrderBy(p => p.Service.Name).ToList(),
                SubscriptionConstants.HeaderCreatedByCode => subscriptions.OrderBy(p => p.CreatedBy.Code).ToList(),
                SubscriptionConstants.HeaderUpdatedByCode => subscriptions.OrderBy(p => p.UpdatedBy.Code).ToList(),
                _ => subscriptions.OrderBy(p => p.Id).ToList(),
            };
        }

        public List<Subscription> SortDescending(string sortBy, List<Subscription> subscriptions)
        {
            return sortBy switch
            {
                SubscriptionConstants.HeaderId => subscriptions.OrderByDescending(p => p.Id).ToList(),
                SubscriptionConstants.HeaderStartDate => subscriptions.OrderByDescending(p => p.StartDate).ToList(),
                SubscriptionConstants.HeaderEndDate => subscriptions.OrderByDescending(p => p.EndDate).ToList(),
                SubscriptionConstants.HeaderTotalPrice => subscriptions.OrderByDescending(p => p.TotalPrice).ToList(),
                SubscriptionConstants.HeaderClientName => subscriptions.OrderByDescending(p => p.Client.Name).ToList(),
                SubscriptionConstants.HeaderServiceName => subscriptions.OrderByDescending(p => p.Service.Name).ToList(),
                SubscriptionConstants.HeaderCreatedByCode => subscriptions.OrderByDescending(p => p.CreatedBy.Code).ToList(),
                SubscriptionConstants.HeaderUpdatedByCode => subscriptions.OrderByDescending(p => p.UpdatedBy.Code).ToList(),
                _ => subscriptions.OrderByDescending(p => p.Id).ToList(),
            };
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
                await _context.Subscriptions.AddAsync(subscription);
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
                Subscription subscription = await GetActive(id);
                Subscription subscriptionHistory = new Subscription
                {
                    StartDate = subscription.StartDate,
                    EndDate = subscription.EndDate,
                    TotalPrice = subscription.TotalPrice,
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

                List<int> clientIds = subscription.SubscriptionClients.Select(p => (int)p.ClientId).ToList();
                foreach (var clientId in clientIds)
                {
                    subscriptionHistory.SubscriptionClients.Add(new SubscriptionClient()
                    {
                        ClientId = clientId,
                        Subscription = subscriptionHistory
                    });
                }

                List<int> userIds = subscription.SubscriptionUsers.Select(p => (int)p.UserId).ToList();
                foreach (int userId in userIds)
                {
                    subscriptionHistory.SubscriptionUsers.Add(new SubscriptionUser()
                    {
                        UserId = userId,
                        Subscription = subscriptionHistory
                    });
                }
                await _context.Subscriptions.AddAsync(subscriptionHistory);
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
                var subscriptionUpdate = await GetActive(subscription.Id);
                subscriptionUpdate.StartDate = subscription.StartDate;
                subscriptionUpdate.EndDate = subscription.EndDate;
                subscriptionUpdate.TotalPrice = subscription.TotalPrice;
                subscriptionUpdate.ClientId = subscription.ClientId;
                subscriptionUpdate.ServiceId = subscription.ServiceId;
                subscriptionUpdate.UpdatedOn = subscription.UpdatedOn;
                subscriptionUpdate.UpdatedById = subscription.UpdatedById;

                List<SubscriptionClient> newClients= new List<SubscriptionClient>();
                foreach (int id in clientIds)
                {
                    newClients.Add(new SubscriptionClient()
                    {
                        ClientId = id,
                        SubscriptionId = subscriptionUpdate.Id
                    });
                }
                subscriptionUpdate.SubscriptionClients = newClients;

                List<SubscriptionUser> newUsers = new List<SubscriptionUser>();
                foreach (int id in userIds)
                {
                    newUsers.Add(new SubscriptionUser()
                    {
                        UserId = id,
                        SubscriptionId = subscriptionUpdate.Id
                    });
                }
                subscriptionUpdate.SubscriptionUsers = newUsers;

                _context.Update(subscriptionUpdate);
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
                subscriptionHistory.Add(await GetInactive(id));
                _context.Subscriptions.RemoveRange(subscriptionHistory);
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
                Subscription subscription = await GetActive(id);
                subscription.IsActive = false;
                _context.Subscriptions.Update(subscription);
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
                _context.Subscriptions.RemoveRange(subscriptions);
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
                _context.Subscriptions.UpdateRange(subscriptions);
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
                Subscription subscription = await GetInactive(id);
                subscription.IsActive = true;
                _context.Subscriptions.Update(subscription);
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
                _context.Subscriptions.UpdateRange(subscriptions);
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
                return await _context.Subscriptions.AnyAsync(p => p.ServiceId == subscription.ServiceId && p.ClientId == subscription.ClientId && p.Id != subscription.Id && p.IsActive == true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
