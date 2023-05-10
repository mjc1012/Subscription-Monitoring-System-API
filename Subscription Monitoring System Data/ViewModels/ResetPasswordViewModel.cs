namespace Subscription_Monitoring_System_Data.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string EmailAddress { get; set; } = string.Empty;
        public string ResetPasswordToken { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;

    }
}
