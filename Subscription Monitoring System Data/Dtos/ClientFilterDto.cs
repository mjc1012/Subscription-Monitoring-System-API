using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Dtos
{
    public class ClientFilterDto
    {
        public int Page { get; set; }
        public string SortBy { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; 
        public string EmailAddress { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
