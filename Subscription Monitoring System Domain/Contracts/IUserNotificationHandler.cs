using Subscription_Monitoring_System_Data.ViewModels;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IUserNotificationHandler
    {
        Task<List<string>> CanDeleteActive(int id);
        Task<List<string>> CanDeleteInactive(int id);
        Task<List<string>> CanDeleteActive(RecordIdsViewModel records);
        Task<List<string>> CanDeleteInactive(RecordIdsViewModel records);
        Task<List<string>> CanRestore(int id);
        Task<List<string>> CanRestore(RecordIdsViewModel records);
    }
}
