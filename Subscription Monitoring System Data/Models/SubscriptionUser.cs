namespace Subscription_Monitoring_System_Data.Models
{
    public class SubscriptionUser
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
