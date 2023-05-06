using AutoMapper;
using Org.BouncyCastle.Asn1.Ocsp;
using Subscription_Monitoring_System_Data;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.Repositories;
using Subscription_Monitoring_System_Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<SubscriptionDto> GetActive(int id)
        {
            try
            {
                Subscription subscription = await _subscriptionRepository.GetActive(id);
                SubscriptionDto subscriptionMapped = _mapper.Map<SubscriptionDto>(subscription);

                if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.DAILY) subscriptionMapped.TotalPrice = (subscription.EndDate.Date - subscription.StartDate.Date).Days * subscription.Service.Price;
                if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.WEEKLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscription.EndDate.Date - subscription.StartDate.Date).Days / 7.0) * subscription.Service.Price;
                if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.MONTHLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscription.EndDate.Date - subscription.StartDate.Date).Days / 30.0) * subscription.Service.Price;
                if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.YEARLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscription.EndDate.Date - subscription.StartDate.Date).Days / 365.0) * subscription.Service.Price;
                return subscriptionMapped;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SubscriptionDto> CreateHistory(int id)
        {
            try
            {
                return _mapper.Map<SubscriptionDto>(await _subscriptionRepository.CreateHistory(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SubscriptionDto> GetInactive(int id)
        {
            try
            {
                Subscription subscription = await _subscriptionRepository.GetInactive(id);
                SubscriptionDto subscriptionMapped = _mapper.Map<SubscriptionDto>(subscription);
                if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.DAILY) subscriptionMapped.TotalPrice = (subscription.EndDate.Date - subscription.StartDate.Date).Days * subscription.Service.Price;
                if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.WEEKLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscription.EndDate.Date - subscription.StartDate.Date).Days / 7.0) * subscription.Service.Price;
                if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.MONTHLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscription.EndDate.Date - subscription.StartDate.Date).Days / 30.0) * subscription.Service.Price;
                if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.YEARLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscription.EndDate.Date - subscription.StartDate.Date).Days / 365.0) * subscription.Service.Price;
                return subscriptionMapped;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendExpiringSubscriptionNotification()
        {
            List<SubscriptionDto> subscriptions = _mapper.Map<List<SubscriptionDto>>(await _subscriptionRepository.GetActiveList());
            foreach (SubscriptionDto subscription in subscriptions)
            {   
                if (subscription.RemainingDays == 0)
                {
                    await _subscriptionRepository.Expired(subscription.Id);

                    NotificationDto notification = new()
                    {
                        Description = NotificationConstants.SubscriptionExpired(subscription.Id),
                        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        IsActive = true,
                        SubscriptionId = subscription.Id,
                    };
                    await _notificationService.Create(notification, subscription.UserRecipients.Select(p => p.Id).ToList());

                    foreach (ClientDto clientRecipient in subscription.ClientRecipients)
                    {
                        _emailService.SendEmail(new EmailDto(clientRecipient.EmailAddress, "Subscription is Expired", EmailBody.SendSubscriptionExpiryEmail("Subscription is Expired", NotificationConstants.SubscriptionExpired(subscription.Id))));
                    }
                }
                else if (subscription.RemainingDays == 7)
                {
                    NotificationDto notification = new()
                    {
                        Description = NotificationConstants.SubscriptionExpiring(subscription.Id, subscription.RemainingDays),
                        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        IsActive = true,
                        SubscriptionId = subscription.Id,
                    };
                    await _notificationService.Create(notification, subscription.UserRecipients.Select(p => p.Id).ToList());

                    foreach (ClientDto clientRecipient in subscription.ClientRecipients)
                    {
                        _emailService.SendEmail(new EmailDto(clientRecipient.EmailAddress, "Subscription is Expiring", EmailBody.SendSubscriptionExpiryEmail("Subscription is Expiring", NotificationConstants.SubscriptionExpiring(subscription.Id, subscription.RemainingDays))));
                    }
                }
            }
        }

        public async Task<ListDto> GetList(SubscriptionFilterDto filter)
        {
            try
            {
                List<Subscription> subscriptions = await _subscriptionRepository.GetActiveList();
                List<SubscriptionDto> subscriptionsMapped = new List<SubscriptionDto>();
                foreach(Subscription subscription in subscriptions)
                {
                    SubscriptionDto subscriptionMapped = _mapper.Map<SubscriptionDto>(subscription);
                    if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.DAILY) subscriptionMapped.TotalPrice = (subscription.EndDate.Date - subscription.StartDate.Date).Days * subscription.Service.Price;
                    if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.WEEKLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscription.EndDate.Date - subscription.StartDate.Date).Days / 7.0) * subscription.Service.Price;
                    if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.MONTHLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscription.EndDate.Date - subscription.StartDate.Date).Days / 30.0) * subscription.Service.Price;
                    if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.YEARLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscription.EndDate.Date - subscription.StartDate.Date).Days / 365.0) * subscription.Service.Price;
                    subscriptionsMapped.Add(subscriptionMapped);
                }

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

                return new ListDto { Pagination = pagination, Data = subscriptionsMapped };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SubscriptionDto> SortAscending(string sortBy, List<SubscriptionDto> subscriptions)
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

        public List<SubscriptionDto> SortDescending(string sortBy, List<SubscriptionDto> subscriptions)
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

        public async Task<List<SubscriptionDto>> GetList(List<int> ids)
        {
            try
            {
                List<Subscription> subscriptions = await _subscriptionRepository.GetList(ids);
                List<SubscriptionDto> subscriptionsMapped = new List<SubscriptionDto>();
                foreach (Subscription subscription in subscriptions)
                {
                    SubscriptionDto subscriptionMapped = _mapper.Map<SubscriptionDto>(subscription);
                    if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.DAILY) subscriptionMapped.TotalPrice = (subscription.EndDate.Date - subscription.StartDate.Date).Days * subscription.Service.Price;
                    if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.WEEKLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscription.EndDate.Date - subscription.StartDate.Date).Days / 7.0) * subscription.Service.Price;
                    if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.MONTHLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscription.EndDate.Date - subscription.StartDate.Date).Days / 30.0) * subscription.Service.Price;
                    if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.YEARLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscription.EndDate.Date - subscription.StartDate.Date).Days / 365.0) * subscription.Service.Price;
                    subscriptionsMapped.Add(subscriptionMapped);
                }
                return subscriptionsMapped;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SubscriptionDto>> GetHistoryList(int id)
        {
            try
            {
                List<Subscription> subscriptions = await _subscriptionRepository.GetHistoryList(id);
                List<SubscriptionDto> subscriptionsMapped = new List<SubscriptionDto>();
                foreach (Subscription subscription in subscriptions)
                {
                    SubscriptionDto subscriptionMapped = _mapper.Map<SubscriptionDto>(subscription);
                    if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.DAILY) subscriptionMapped.TotalPrice = (subscription.EndDate.Date - subscription.StartDate.Date).Days * subscription.Service.Price;
                    if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.WEEKLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscription.EndDate.Date - subscription.StartDate.Date).Days / 7.0) * subscription.Service.Price;
                    if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.MONTHLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscription.EndDate.Date - subscription.StartDate.Date).Days / 30.0) * subscription.Service.Price;
                    if (subscription.Service.ServiceType.Name == SubscriptionTypeConstants.YEARLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscription.EndDate.Date - subscription.StartDate.Date).Days / 365.0) * subscription.Service.Price;
                    subscriptionsMapped.Add(subscriptionMapped);
                }
                return subscriptionsMapped;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SubscriptionDto> Create(SubscriptionDto subscription, ClientDto client, ServiceDto service, UserDto createdBy)
        {
            try
            {

                Subscription subscriptionMapped = _mapper.Map<Subscription>(subscription);
                subscriptionMapped.ClientId = client.Id;
                subscriptionMapped.ServiceId = service.Id;
                subscriptionMapped.CreatedById = createdBy.Id;
                subscriptionMapped.CreatedOn = DateTime.Now;
                return _mapper.Map<SubscriptionDto>(await _subscriptionRepository.Create(subscriptionMapped, subscription.ClientIds, subscription.UserIds));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SubscriptionDto> Update(SubscriptionDto subscription, ClientDto client, ServiceDto service, UserDto updatedBy)
        {
            try
            {
                Subscription subscriptionMapped = _mapper.Map<Subscription>(subscription);
                subscriptionMapped.ClientId = client.Id;
                subscriptionMapped.ServiceId = service.Id;
                subscriptionMapped.UpdatedById = updatedBy.Id;
                subscriptionMapped.UpdatedOn = DateTime.Now;
                return _mapper.Map<SubscriptionDto>(await _subscriptionRepository.Update(subscriptionMapped, subscription.ClientIds, subscription.UserIds));
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

        public async Task HardDelete(RecordIdsDto records)
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

        public async Task SoftDelete(RecordIdsDto records)
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

        public async Task Restore(RecordIdsDto records)
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

        public async Task<bool> SubscriptionExists(SubscriptionDto subscription)
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
