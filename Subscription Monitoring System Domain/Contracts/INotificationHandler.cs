using Subscription_Monitoring_System_Data.ViewModels;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface INotificationHandler
    {
        Task<List<string>> CanAdd(NotificationViewModel notification);
        Task<List<string>> CanUpdate(NotificationViewModel notification);
        Task<List<string>> CanDelete(int id);
        Task<List<string>> CanDelete(RecordIdsViewModel records);
    }
}
