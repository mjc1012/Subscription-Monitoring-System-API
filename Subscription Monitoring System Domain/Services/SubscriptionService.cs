using AutoMapper;
using Subscription_Monitoring_System_Data;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, IMapper mapper, INotificationService notificationService, IEmailService emailService)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
            _notificationService = notificationService;
            _emailService = emailService;
        }

        public async Task<SubscriptionViewModel> GetActive(int id)
        {
            try
            {
                Subscription subscription = await _subscriptionRepository.GetActive(id);
                return _mapper.Map<SubscriptionViewModel>(subscription);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SubscriptionViewModel> CreateHistory(int id)
        {
            try
            {
                return _mapper.Map<SubscriptionViewModel>(await _subscriptionRepository.CreateHistory(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SubscriptionViewModel> GetInactive(int id)
        {
            try
            {
                Subscription subscription = await _subscriptionRepository.GetInactive(id);
                return _mapper.Map<SubscriptionViewModel>(subscription);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendExpiringSubscriptionNotification()
        {
            List<SubscriptionViewModel> subscriptions = _mapper.Map<List<SubscriptionViewModel>>(await _subscriptionRepository.GetList());
            foreach (SubscriptionViewModel subscription in subscriptions)
            {   
                if(!subscription.IsActive) continue;

                if (subscription.RemainingDays == 0)
                {
                    await _subscriptionRepository.Expired(subscription.Id);

                    NotificationViewModel notification = new()
                    {
                        Description = NotificationConstants.SubscriptionExpired(subscription.Id),
                        Date = DateTime.Now.ToString(DateConstants.DateTimeFormat),
                        IsActive = true,
                        SubscriptionId = subscription.Id,
                        UserIds = subscription.UserRecipients.Select(p => p.Id).ToList()
                    };
                    await _notificationService.Create(notification);

                    foreach (ClientViewModel clientRecipient in subscription.ClientRecipients)
                    {
                        _emailService.SendEmail(new EmailViewModel(clientRecipient.EmailAddress, NotificationConstants.ExpiredSubject, EmailBody.SendSubscriptionEmail(NotificationConstants.ExpiredSubject, NotificationConstants.SubscriptionExpired(subscription.Id))));
                    }
                }
                else if (subscription.RemainingDays == 7)
                {
                    NotificationViewModel notification = new()
                    {
                        Description = NotificationConstants.SubscriptionExpiring(subscription.Id, subscription.RemainingDays),
                        Date = DateTime.Now.ToString(DateConstants.DateTimeFormat),
                        IsActive = true,
                        SubscriptionId = subscription.Id,
                        UserIds = subscription.UserRecipients.Select(p => p.Id).ToList()
                    };
                    await _notificationService.Create(notification);

                    foreach (ClientViewModel clientRecipient in subscription.ClientRecipients)
                    {
                        _emailService.SendEmail(new EmailViewModel(clientRecipient.EmailAddress, NotificationConstants.ExpiringSubject, EmailBody.SendSubscriptionEmail(NotificationConstants.ExpiringSubject, NotificationConstants.SubscriptionExpiring(subscription.Id, subscription.RemainingDays))));
                    }
                }
            }
        }

        public async Task<ListViewModel> GetList(SubscriptionFilterViewModel filter)
        {
            try
            {
                if (filter.Page == 0) filter.Page = 1;

                List<Subscription> subscriptions = await _subscriptionRepository.GetList();
                List<SubscriptionViewModel> subscriptionsMapped = _mapper.Map<List<SubscriptionViewModel>>(subscriptions);

                subscriptionsMapped = subscriptionsMapped.Where(p => (filter.RemainingDays == 0 || p.RemainingDays == filter.RemainingDays) && 
                (filter.TotalPrice == 0 || p.TotalPrice == filter.TotalPrice) &&
                (filter.Id == 0 || p.Id == filter.Id) &&
                (string.IsNullOrEmpty(filter.StartDate) || p.StartDate == filter.StartDate) &&
                (string.IsNullOrEmpty(filter.EndDate) || p.EndDate == filter.EndDate) &&
                (string.IsNullOrEmpty(filter.ClientName) || p.ClientName == filter.ClientName) &&
                (string.IsNullOrEmpty(filter.ServiceName) || p.ServiceName == filter.ServiceName) &&
                (string.IsNullOrEmpty(filter.CreatedByCode) || p.CreatedByCode == filter.CreatedByCode) &&
                (string.IsNullOrEmpty(filter.UpdatedByCode) || p.UpdatedByCode == filter.UpdatedByCode) &&
                p.IsActive == filter.IsActive && 
                p.IsExpired == filter.IsExpired).ToList();
                subscriptionsMapped = (!string.IsNullOrEmpty(filter.SortOrder) && filter.SortOrder.Equals(SortDirectionConstants.Descending)) ? SortDescending(filter.SortBy, subscriptionsMapped) : SortAscending(filter.SortBy, subscriptionsMapped);

                int totalCount = subscriptionsMapped.Count;
                int totalPages = (int)Math.Ceiling((double)totalCount / BaseConstants.PageSize);
                var pagination = new
                {
                    pages = totalPages,
                    size = totalCount
                };

                subscriptionsMapped = subscriptionsMapped.Skip(BaseConstants.PageSize * (filter.Page - 1)).Take(BaseConstants.PageSize).ToList();

                return new ListViewModel { Pagination = pagination, Data = subscriptionsMapped };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SubscriptionViewModel> SortAscending(string sortBy, List<SubscriptionViewModel> subscriptions)
        {
            return sortBy switch
            {
                SubscriptionConstants.HeaderId => subscriptions.OrderBy(p => p.Id).ToList(),
                SubscriptionConstants.HeaderStartDate => subscriptions.OrderBy(p => p.StartDate).ToList(),
                SubscriptionConstants.HeaderEndDate => subscriptions.OrderBy(p => p.EndDate).ToList(),
                SubscriptionConstants.HeaderRemainingDays => subscriptions.OrderBy(p => p.RemainingDays).ToList(),
                SubscriptionConstants.HeaderTotalPrice => subscriptions.OrderBy(p => p.TotalPrice).ToList(),
                SubscriptionConstants.HeaderClientName => subscriptions.OrderBy(p => p.ClientName).ToList(),
                SubscriptionConstants.HeaderServiceName => subscriptions.OrderBy(p => p.ServiceName).ToList(),
                SubscriptionConstants.HeaderCreatedByCode => subscriptions.OrderBy(p => p.CreatedByCode).ToList(),
                SubscriptionConstants.HeaderUpdatedByCode => subscriptions.OrderBy(p => p.UpdatedByCode).ToList(),
                _ => subscriptions.OrderBy(p => p.Id).ToList(),
            };
        }

        public List<SubscriptionViewModel> SortDescending(string sortBy, List<SubscriptionViewModel> subscriptions)
        {
            return sortBy switch
            {
                SubscriptionConstants.HeaderId => subscriptions.OrderByDescending(p => p.Id).ToList(),
                SubscriptionConstants.HeaderStartDate => subscriptions.OrderByDescending(p => p.StartDate).ToList(),
                SubscriptionConstants.HeaderEndDate => subscriptions.OrderByDescending(p => p.EndDate).ToList(),
                SubscriptionConstants.HeaderRemainingDays => subscriptions.OrderByDescending(p => p.RemainingDays).ToList(),
                SubscriptionConstants.HeaderTotalPrice => subscriptions.OrderByDescending(p => p.TotalPrice).ToList(),
                SubscriptionConstants.HeaderClientName => subscriptions.OrderByDescending(p => p.ClientName).ToList(),
                SubscriptionConstants.HeaderServiceName => subscriptions.OrderByDescending(p => p.ServiceName).ToList(),
                SubscriptionConstants.HeaderCreatedByCode => subscriptions.OrderByDescending(p => p.CreatedByCode).ToList(),
                SubscriptionConstants.HeaderUpdatedByCode => subscriptions.OrderByDescending(p => p.UpdatedByCode).ToList(),
                _ => subscriptions.OrderByDescending(p => p.Id).ToList(),
            };
        }

        public async Task<List<SubscriptionViewModel>> GetList(List<int> ids)
        {
            try
            {
                List<Subscription> subscriptions = await _subscriptionRepository.GetList(ids);
                return _mapper.Map<List<SubscriptionViewModel>>(subscriptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SubscriptionViewModel>> GetHistoryList(int id)
        {
            try
            {
                List<Subscription> subscriptions = await _subscriptionRepository.GetHistoryList(id);
                return _mapper.Map<List<SubscriptionViewModel>>(subscriptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SubscriptionViewModel> Create(SubscriptionViewModel subscription, ClientViewModel client, ServiceViewModel service, UserViewModel createdBy)
        {
            try
            {

                Subscription subscriptionMapped = _mapper.Map<Subscription>(subscription);
                subscriptionMapped.ClientId = client.Id;
                subscriptionMapped.ServiceId = service.Id;
                subscriptionMapped.CreatedById = createdBy.Id;
                subscriptionMapped.CreatedOn = DateTime.Now;
                return _mapper.Map<SubscriptionViewModel>(await _subscriptionRepository.Create(subscriptionMapped, subscription.ClientIds, subscription.UserIds));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SubscriptionViewModel> Update(SubscriptionViewModel subscription, ClientViewModel client, ServiceViewModel service, UserViewModel updatedBy)
        {
            try
            {
                Subscription subscriptionMapped = _mapper.Map<Subscription>(subscription);
                subscriptionMapped.ClientId = client.Id;
                subscriptionMapped.ServiceId = service.Id;
                subscriptionMapped.UpdatedById = updatedBy.Id;
                subscriptionMapped.UpdatedOn = DateTime.Now;
                return _mapper.Map<SubscriptionViewModel>(await _subscriptionRepository.Update(subscriptionMapped, subscription.ClientIds, subscription.UserIds));
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
                await _subscriptionRepository.HardDelete(id);
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
                await _subscriptionRepository.SoftDelete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task HardDelete(RecordIdsViewModel records)
        {
            try
            {
                await _subscriptionRepository.HardDelete(records.Ids);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SoftDelete(RecordIdsViewModel records)
        {
            try
            {
                await _subscriptionRepository.SoftDelete(records.Ids);
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
                await _subscriptionRepository.Restore(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Restore(RecordIdsViewModel records)
        {
            try
            {
                await _subscriptionRepository.Restore(records.Ids);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SubscriptionExists(SubscriptionViewModel subscription)
        {
            try
            {
                return await _subscriptionRepository.SubscriptionExists(_mapper.Map<Subscription>(subscription));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
