using Microsoft.AspNetCore.Mvc;
using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface ISubscriptionService
    {
        Task<SubscriptionDto> GetActive(int id);
        Task<SubscriptionDto> GetInactive(int id);
        List<SubscriptionDto> SortAscending(string sortBy, List<SubscriptionDto> subscriptions);
        List<SubscriptionDto> SortDescending(string sortBy, List<SubscriptionDto> subscriptions);

        Task<ListDto> GetList(SubscriptionFilterDto filter);
        Task<List<SubscriptionDto>> GetList(List<int> ids);
        Task<List<SubscriptionDto>> GetHistoryList(int id);
        Task SendExpiringSubscriptionNotification();
        Task<SubscriptionDto> Create(SubscriptionDto subscription, ClientDto client, ServiceDto service, UserDto createdBy);
        Task<SubscriptionDto> Update(SubscriptionDto subscription, ClientDto client, ServiceDto service, UserDto updatedBy);
        Task SoftDelete(int id);
        Task HardDelete(int id);
        Task SoftDelete(RecordIdsDto records);
        Task HardDelete(RecordIdsDto records);
        Task Restore(int id);
        Task Restore(RecordIdsDto records);
        Task<bool> SubscriptionExists(SubscriptionDto subscription);
        Task<SubscriptionDto> CreateHistory(int id);
    }
}
