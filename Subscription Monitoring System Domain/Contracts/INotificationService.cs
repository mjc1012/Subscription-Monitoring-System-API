using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface INotificationService
    {
        Task<NotificationViewModel> Get(int id);
        Task<List<NotificationViewModel>> GetList(List<int> ids);
        Task Create(NotificationViewModel notification, List<int> userIds);
        Task HardDelete(int id);
        Task HardDelete(RecordIdsViewModel records);
    }
}
