namespace Subscription_Monitoring_System_Data.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public int ServiceDurationId { get; set; }
        public virtual ServiceDuration ServiceDuration { get; set; } = null!;
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
