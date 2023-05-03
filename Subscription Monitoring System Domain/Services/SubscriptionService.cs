using AutoMapper;
using Org.BouncyCastle.Asn1.Ocsp;
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
        private readonly IMapper _mapper;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }

        public async Task<SubscriptionDto> GetActive(int id)
        {
            try
            {
                return _mapper.Map<SubscriptionDto>(await _subscriptionRepository.GetActive(id));
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
                return _mapper.Map<SubscriptionDto>(await _subscriptionRepository.GetInactive(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ListDto> GetList(SubscriptionFilterDto filter)
        {
            try
            {
                List<SubscriptionDto> subscriptions = _mapper.Map<List<SubscriptionDto>>(await _subscriptionRepository.GetList(filter));
                foreach(SubscriptionDto subscription in subscriptions)
                {
                    DateTime endDate = DateTime.ParseExact(subscription.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    subscription.RemainingDays = (endDate - DateTime.Now.Date).Days;
                }

                subscriptions = subscriptions.Where(p => filter.RemainingDays == 0 || p.RemainingDays == filter.RemainingDays).ToList();

                int totalCount = subscriptions.Count;
                int totalPages = (int)Math.Ceiling((double)totalCount / BaseConstants.PageSize);
                var pagination = new
                {
                    pages = totalPages,
                    size = totalCount
                };

                subscriptions = subscriptions.Skip(BaseConstants.PageSize * (filter.Page - 1)).Take(BaseConstants.PageSize).ToList();

                return new ListDto { Pagination = pagination, Data = subscriptions };
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<SubscriptionDto>> GetList(List<int> ids)
        {
            try
            {
                return _mapper.Map<List<SubscriptionDto>>(await _subscriptionRepository.GetList(ids));
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
                return _mapper.Map<List<SubscriptionDto>>(await _subscriptionRepository.GetHistoryList(id));
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
                if (service.ServiceTypeName == SubscriptionTypeConstants.DAILY) subscriptionMapped.TotalPrice = (subscriptionMapped.EndDate.Date - subscriptionMapped.StartDate.Date).Days * service.Price;
                if (service.ServiceTypeName == SubscriptionTypeConstants.WEEKLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscriptionMapped.EndDate.Date - subscriptionMapped.StartDate.Date).Days / 7.0) * service.Price;
                if (service.ServiceTypeName == SubscriptionTypeConstants.MONTHLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscriptionMapped.EndDate.Date - subscriptionMapped.StartDate.Date).Days / 30.0) * service.Price;
                if (service.ServiceTypeName == SubscriptionTypeConstants.YEARLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscriptionMapped.EndDate.Date - subscriptionMapped.StartDate.Date).Days / 365.0) * service.Price;
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
                if (service.ServiceTypeName == SubscriptionTypeConstants.DAILY) subscriptionMapped.TotalPrice = (subscriptionMapped.EndDate.Date - subscriptionMapped.StartDate.Date).Days * service.Price;
                if (service.ServiceTypeName == SubscriptionTypeConstants.WEEKLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscriptionMapped.EndDate.Date - subscriptionMapped.StartDate.Date).Days / 7.0) * service.Price;
                if (service.ServiceTypeName == SubscriptionTypeConstants.MONTHLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscriptionMapped.EndDate.Date - subscriptionMapped.StartDate.Date).Days / 30.0) * service.Price;
                if (service.ServiceTypeName == SubscriptionTypeConstants.YEARLY) subscriptionMapped.TotalPrice = Math.Ceiling((subscriptionMapped.EndDate.Date - subscriptionMapped.StartDate.Date).Days / 365.0) * service.Price;
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
