using Subscription_Monitoring_System_Data.ViewModels;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface ISubscriptionHandler
    {
        List<string> CanFilter(SubscriptionFilterViewModel filter);
        Task<List<string>> CanAdd(SubscriptionViewModel subscription);
        Task<List<string>> CanUpdate(SubscriptionViewModel subscription);
        Task<List<string>> CanDeleteActive(int id);
        Task<List<string>> CanDeleteInactive(int id);
        Task<List<string>> CanDelete(RecordIdsViewModel records);
        Task<List<string>> CanRestore(int id);
        Task<List<string>> CanRestore(RecordIdsViewModel records);
    }
}
