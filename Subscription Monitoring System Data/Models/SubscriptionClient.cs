namespace Subscription_Monitoring_System_Data.Models
{
    public class SubscriptionClient
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; } = null!;
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;
    }
}
