using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IUnitOfWork
    {
        IClientService ClientService { get; }
        IClientHandler ClientHandler { get; }
        IServiceTypeService ServiceTypeService { get; }

        IServiceService ServiceService { get; }
        IServiceHandler ServiceHandler { get; }

        IDepartmentService DepartmentService { get; }

        IUserService UserService { get; }
        IUserHandler UserHandler { get; }

        ISubscriptionService SubscriptionService { get; }
        ISubscriptionHandler SubscriptionHandler { get; }
        IImageService ImageService { get; }

        IAuthenticationService AuthenticationService { get; }
        IAuthenticationHandler AuthenticationHandler { get; }

        INotificationService NotificationService { get; }
        INotificationHandler NotificationHandler { get; }

        IUserNotificationService UserNotificationService { get; }
        IUserNotificationHandler UserNotificationHandler { get;}

        IEmailService EmailService { get; }
    }
}
