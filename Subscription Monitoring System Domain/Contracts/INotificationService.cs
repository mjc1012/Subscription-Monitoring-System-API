using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface INotificationService
    {
        Task<List<NotificationViewModel>> GetList();
        Task<NotificationViewModel> Get(int id);
        Task<List<NotificationViewModel>> GetList(List<int> ids);
        Task Create(NotificationViewModel notification);
        Task Update(NotificationViewModel notification);
        Task HardDelete(int id);
        Task HardDelete(RecordIdsViewModel records);
        Task<bool> NotificationExists(NotificationViewModel notification);
    }
}
