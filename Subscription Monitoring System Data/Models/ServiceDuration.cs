namespace Subscription_Monitoring_System_Data.Models
{
    public class ServiceDuration
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Days { get; set; }
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
