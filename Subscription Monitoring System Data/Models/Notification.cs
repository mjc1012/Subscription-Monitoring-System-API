using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int SubscriptionId { get; set; }
        public virtual Subscription Subscription { get; set; } = null!;
        public virtual ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();
    }
}
