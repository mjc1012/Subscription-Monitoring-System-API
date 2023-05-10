using AutoMapper;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Domain.Contracts;
using Subscription_Monitoring_System_Domain.Handlers;
using Subscription_Monitoring_System_Domain.Services;

namespace Subscription_Monitoring_System_Domain
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceDurationRepository _serviceTypeRepository;
        private readonly IDepartmentRepository _deparmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;

        public UnitOfWork(IMapper mapper, IClientRepository clientRepository, IServiceRepository serviceRepository, 
            IServiceDurationRepository serviceTypeRepository, IDepartmentRepository deparmentRepository, IUserRepository userRepository, ISubscriptionRepository subscriptionRepository,
            INotificationRepository notificationRepository, IUserNotificationRepository userNotificationRepository)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
            _serviceRepository = serviceRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _deparmentRepository = deparmentRepository;
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
            _notificationRepository = notificationRepository;
            _userNotificationRepository = userNotificationRepository;
        }

        public IEmailService EmailService => new EmailService();
        public IClientService ClientService => new ClientService(_clientRepository,_mapper);
        public IClientHandler ClientHandler => new ClientHandler(ClientService, EmailService);
        public IServiceDurationService ServiceTypeService => new ServiceDurationService(_serviceTypeRepository, _mapper);
        public IServiceDurationHandler ServiceTypeHandler => new ServiceDurationHandler(ServiceTypeService);
        public IServiceService ServiceService => new ServiceService(_serviceRepository, _mapper);
        public IServiceHandler ServiceHandler => new ServiceHandler(ServiceService, ServiceTypeService);
        public IDepartmentService DepartmentService => new DepartmentService(_deparmentRepository, _mapper);
        public IDepartmentHandler DepartmentHandler => new DepartmentHandler(DepartmentService);
        public IUserService UserService => new UserService(_userRepository, _mapper);
        public IUserHandler UserHandler => new UserHandler(UserService, EmailService, DepartmentService);
        public ISubscriptionService SubscriptionService => new SubscriptionService(_subscriptionRepository, _mapper, NotificationService, EmailService);
        public ISubscriptionHandler SubscriptionHandler => new SubscriptionHandler(SubscriptionService, UserService, ClientService, ServiceService);
        public IImageService ImageService => new ImageService();
        public IAuthenticationService AuthenticationService => new AuthenticationService(_userRepository,_mapper);
        public IAuthenticationHandler AuthenticationHandler => new AuthenticationHandler(AuthenticationService, UserService);
        public INotificationService NotificationService => new NotificationService(_notificationRepository,_mapper);
        public INotificationHandler NotificationHandler => new NotificationHandler(NotificationService);
        public IUserNotificationService UserNotificationService => new UserNotificationService(_userNotificationRepository, _mapper);
        public IUserNotificationHandler UserNotificationHandler => new UserNotificationHandler(UserNotificationService);
    }
}
