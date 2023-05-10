namespace Subscription_Monitoring_System_Data.ViewModels
{
    public class ResponseViewModel
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public object Value { get; set; } = null!;
    }
}
