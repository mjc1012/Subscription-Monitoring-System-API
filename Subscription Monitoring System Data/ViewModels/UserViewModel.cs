using Microsoft.AspNetCore.Http;

namespace Subscription_Monitoring_System_Data.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string ProfilePictureImageName { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }
        public string Code { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int UnseenNotifications { get; set; }
    }
}
