using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.ViewModels
{
    public class ServiceFilterViewModel
    {
        public int Page { get; set; }
        public string SortBy { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
        public string ServiceTypeName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
