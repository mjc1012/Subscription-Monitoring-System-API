using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalPrice { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int CreatedById { get; set; }
        public virtual User CreatedBy { get; set; } = null!;
        public int? UpdatedById { get; set; }
        public virtual User? UpdatedBy { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; } = null!;
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; } = null!;
        public int? SubscriptionHistoryId { get; set; }
        public virtual Subscription? SubscriptionHistory { get; set; } 
        public virtual ICollection<Subscription> SubscriptionHistories { get; set; } = new List<Subscription>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<SubscriptionUser> SubscriptionUsers { get; set; } = new List<SubscriptionUser>();
        public virtual ICollection<SubscriptionClient> SubscriptionClients { get; set; } = new List<SubscriptionClient>(); 
    }
}
