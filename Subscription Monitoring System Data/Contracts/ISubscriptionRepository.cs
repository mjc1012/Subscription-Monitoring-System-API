using Subscription_Monitoring_System_Data.Models;

namespace Subscription_Monitoring_System_Data.Contracts
{
    public interface ISubscriptionRepository
    {
        Task<Subscription?> GetActive(int id);
        Task<Subscription?> GetInactive(int id);
        Task<List<Subscription>> GetList(List<int> ids);
        Task<List<Subscription>> GetHistoryList(int id);
        Task<List<Subscription>> GetList();
        Task Expired(int id);
        Task<Subscription> Create(Subscription subscription, List<int> clientIds, List<int> userIds);
        Task<Subscription> CreateHistory(int id);
        Task<Subscription> Update(Subscription subscriptionn, List<int> clientIds, List<int> userIds);
        Task SoftDelete(int id);
        Task HardDelete(int id);
        Task SoftDelete(List<int> ids);
        Task HardDelete(List<int> ids);
        Task Restore(int id);
        Task Restore(List<int> ids);
        Task<bool> SubscriptionExists(Subscription subscription);
    }
}
