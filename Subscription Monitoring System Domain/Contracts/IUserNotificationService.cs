using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IUserNotificationService
    {
        Task<UserNotification> GetActive(int id);
        Task<UserNotification> GetInactive(int id);
        Task<List<NotificationViewModel>> GetList(int userId);
        Task<List<UserNotification>> GetList(List<int> ids);
        Task SoftDelete(int id);
        Task HardDelete(int id);
        Task SoftDelete(RecordIdsViewModel records);
        Task HardDelete(RecordIdsViewModel records);
        Task Restore(int id);
        Task Restore(RecordIdsViewModel records);
    }
}
