namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IUnitOfWork
    {
        IClientService ClientService { get; }
        IClientHandler ClientHandler { get; }
        IServiceDurationService ServiceTypeService { get; }
        IServiceDurationHandler ServiceTypeHandler { get; }

        IServiceService ServiceService { get; }
        IServiceHandler ServiceHandler { get; }

        IDepartmentService DepartmentService { get; }
        IDepartmentHandler DepartmentHandler { get; }

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
