using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface INotificationService
    {
        Task<NotificationDto> Get(int id);
        Task<List<NotificationDto>> GetList(List<int> ids);
        Task Create(NotificationDto notification, List<int> userIds);
        Task HardDelete(int id);
        Task HardDelete(RecordIdsDto records);
    }
}
