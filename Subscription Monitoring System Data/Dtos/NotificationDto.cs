using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public bool IsSeen { get; set; }
        public bool IsActive { get; set; }
        public int SubscriptionId { get; set; }
    }
}
