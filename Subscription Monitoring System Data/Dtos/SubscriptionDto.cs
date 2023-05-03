using Subscription_Monitoring_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Dtos
{
    public class SubscriptionDto
    {
        public int Id { get; set; }
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public double TotalPrice { get; set; }
        public int RemainingDays { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
        public string CreatedByCode { get; set; } = string.Empty;
        public string CreatedByName { get; set; } = string.Empty;        
        public string UpdatedOn { get; set; } = string.Empty;
        public string UpdatedByCode { get; set; } = string.Empty;
        public string UpdatedByName { get; set; } = string.Empty;
        public bool IsExpired { get; set; }
        public bool IsActive { get; set; }
        public List<int> ClientIds { get; set; } = new List<int>();
        public List<int> UserIds { get; set; } = new List<int>();
        public List<UserDto> UserRecipients { get; set; } = new List<UserDto>();
        public List<ClientDto> ClientRecipients { get; set; } = new List<ClientDto>();
    }
}
