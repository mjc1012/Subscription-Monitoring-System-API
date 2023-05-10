using Subscription_Monitoring_System_Data.Models;

namespace Subscription_Monitoring_System_Data.Contracts
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetList();
        Task<Notification?> Get(int id);
        Task<List<Notification>> GetList(List<int> ids);
        Task Create(Notification notification, List<int> userIds);
        Task Update(Notification notification, List<int> userIds);
        Task HardDelete(int id);
        Task HardDelete(List<int> ids);
        Task<bool> NotificationExists(Notification notification);
    }
}
