namespace Subscription_Monitoring_System_Data.Models
{
    public class UserNotification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int NotificationId { get; set; }
        public Notification Notification { get; set; } = null!;
        public bool IsSeen { get; set; }
        public bool IsActive { get; set; }
    }
}
