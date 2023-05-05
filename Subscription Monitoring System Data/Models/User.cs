using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } 
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string ProfilePictureImageName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string ResetPasswordToken { get; set; } = string.Empty;
        public DateTime ResetPasswordExpiry { get; set; }
        public bool IsActive { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<SubscriptionUser> SubscriptionUsers { get; set; } = new List<SubscriptionUser>();
        public virtual ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();

        [InverseProperty("CreatedBy")]
        public virtual ICollection<Subscription>? CreatedSubscriptions { get; set; }

        [InverseProperty("UpdatedBy")]
        public virtual ICollection<Subscription>? UpdatedSubscriptions { get; set; }
    }
}
