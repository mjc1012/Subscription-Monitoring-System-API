using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public int ServiceTypeId { get; set; }
        public virtual ServiceType ServiceType { get; set; } = null!;
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
