using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
