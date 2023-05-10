using Subscription_Monitoring_System_Data.Models;

namespace Subscription_Monitoring_System_Data.Contracts
{
    public interface IUserNotificationRepository
    {
        Task<UserNotification?> GetActive(int id);
        Task<UserNotification?> GetInactive(int id);
        Task<List<UserNotification>> GetList(int userId);
        Task<List<UserNotification>> GetList(List<int> ids);
        Task SoftDelete(int id);
        Task HardDelete(int id);
        Task SoftDelete(List<int> ids);
        Task HardDelete(List<int> ids);
        Task Restore(int id);
        Task Restore(List<int> ids);
    }
}
