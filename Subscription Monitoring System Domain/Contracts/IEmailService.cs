using Subscription_Monitoring_System_Data.ViewModels;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IEmailService
    {
        void SendEmail(EmailViewModel email);
    }
}
