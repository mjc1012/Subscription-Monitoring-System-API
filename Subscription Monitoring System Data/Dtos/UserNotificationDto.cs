using Subscription_Monitoring_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Dtos
{
    public class UserNotificationDto
    {
        public List<NotificationDto> notifications { get; set; } = new List<NotificationDto>();
    }
}
