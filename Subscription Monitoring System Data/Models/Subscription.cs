﻿namespace Subscription_Monitoring_System_Data.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }
        public bool IsExpiring { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int? CreatedById { get; set; }
        public virtual User? CreatedBy { get; set; }
        public int? UpdatedById { get; set; }
        public virtual User? UpdatedBy { get; set; }
        public int? ClientId { get; set; }
        public virtual Client? Client { get; set; }
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
