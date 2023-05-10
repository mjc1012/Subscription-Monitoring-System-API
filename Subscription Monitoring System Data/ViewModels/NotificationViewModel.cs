using Subscription_Monitoring_System_Data.Models;

namespace Subscription_Monitoring_System_Data.ViewModels
{
    public class NotificationViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public bool IsSeen { get; set; }
        public bool IsActive { get; set; }
        public int? SubscriptionId { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();
        public List<User> Users { get; set; } = new List<User>();
    }
}
