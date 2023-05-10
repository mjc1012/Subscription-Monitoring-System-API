using System.ComponentModel.DataAnnotations.Schema;

namespace Subscription_Monitoring_System_Data.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        [InverseProperty("Client")]
        public virtual ICollection<Subscription> InvolvedSubscriptions { get; set; } = new List<Subscription>();
        public virtual ICollection<SubscriptionClient> SubscriptionClients { get; set; } = new List<SubscriptionClient>();
    }
}
