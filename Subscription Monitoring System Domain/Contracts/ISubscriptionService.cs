using Subscription_Monitoring_System_Data.ViewModels;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface ISubscriptionService
    {
        Task<SubscriptionViewModel> GetActive(int id);
        Task<SubscriptionViewModel> GetInactive(int id);
        List<SubscriptionViewModel> SortAscending(string sortBy, List<SubscriptionViewModel> subscriptions);
        List<SubscriptionViewModel> SortDescending(string sortBy, List<SubscriptionViewModel> subscriptions);

        Task<ListViewModel> GetList(SubscriptionFilterViewModel filter);
        Task<List<SubscriptionViewModel>> GetList(List<int> ids);
        Task<List<SubscriptionViewModel>> GetHistoryList(int id);
        Task<List<SubscriptionViewModel>> GetListForExcel(SubscriptionFilterViewModel filter);
        Task SendExpiringSubscriptionNotification();
        Task<SubscriptionViewModel> Create(SubscriptionViewModel subscription, ClientViewModel client, ServiceViewModel service, UserViewModel createdBy);
        Task<SubscriptionViewModel> Update(SubscriptionViewModel subscription, ClientViewModel client, ServiceViewModel service, UserViewModel updatedBy);
        Task SoftDelete(int id);
        Task HardDelete(int id);
        Task SoftDelete(RecordIdsViewModel records);
        Task HardDelete(RecordIdsViewModel records);
        Task Restore(int id);
        Task Restore(RecordIdsViewModel records);
        Task<bool> SubscriptionExists(SubscriptionViewModel subscription);
        Task<SubscriptionViewModel> CreateHistory(int id);
    }
}
