using Subscription_Monitoring_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Contracts
{
    public interface INotificationRepository
    {
        Task<Notification?> Get(int id);
        Task<List<Notification>> GetList(List<int> ids);
        Task Create(Notification notification, List<int> userIds);
        Task HardDelete(int id);
        Task HardDelete(List<int> ids);
    }
}
