namespace Subscription_Monitoring_System_Data.ViewModels
{
    public class ServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
        public string ServiceDurationName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
