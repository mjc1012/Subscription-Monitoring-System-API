using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.ViewModels
{
    public class SubscriptionFilterViewModel
    {
        public int Page { get; set; }
        public string SortBy { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
        public int Id { get; set; }
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public double TotalPrice { get; set; }
        public int RemainingDays { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string CreatedByCode { get; set; } = string.Empty;
        public string UpdatedByCode { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsExpired { get; set; } = false;
    }
}
