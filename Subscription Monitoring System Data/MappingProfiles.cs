using AutoMapper;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Data
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Client, ClientViewModel>();
            CreateMap<ClientViewModel, Client>()
                .ForMember(
                destiny => destiny.Name,
                opt => opt.MapFrom(origin => origin.Name)
                );

            CreateMap<Department, DepartmentViewModel>().ReverseMap();
            CreateMap<DepartmentViewModel, Department>()
                .ForMember(
                destiny => destiny.Name,
                opt => opt.MapFrom(origin => origin.Name)
                );

            CreateMap<ServiceTypeViewModel, ServiceType>();
            CreateMap<ServiceType, ServiceTypeViewModel>()
                .ForMember(
                destiny => destiny.Name,
                opt => opt.MapFrom(origin => origin.Name)
                );

            CreateMap<ServiceViewModel, Service>()
                .ForMember(
                destiny => destiny.Name,
                opt => opt.MapFrom(origin => origin.Name)
                );

            CreateMap<Service, ServiceViewModel>()
                .ForMember(
                destiny => destiny.ServiceTypeName,
                opt => opt.MapFrom(origin => origin.ServiceType.Name)
                );

            CreateMap<User, UserViewModel>()
                 .ForMember(
                destiny => destiny.DepartmentName,
                opt => opt.MapFrom(origin => origin.Department.Name)
                )
                .ForMember(
                destiny => destiny.UnseenNotifications,
                opt => opt.MapFrom(origin => origin.UserNotifications.Where(p=> !p.IsSeen).Count())
                );

            CreateMap<UserViewModel, User>()
                .ForMember(
                destiny => destiny.FirstName,
                opt => opt.MapFrom(origin => origin.FirstName)
                )
                .ForMember(
                destiny => destiny.MiddleName,
                opt => opt.MapFrom(origin => origin.MiddleName)
                )
                .ForMember(
                destiny => destiny.LastName,
                opt => opt.MapFrom(origin => origin.LastName)
                );

            CreateMap<AuthenticationViewModel, User>();

            CreateMap<Notification, NotificationViewModel>()
                .ForMember(
                destiny => destiny.Date,
                opt => opt.MapFrom(origin => origin.Date.ToString(DateConstants.DateTimeFormat))
                );

            CreateMap<UserNotification, NotificationViewModel>()
                .ForMember(
                destiny => destiny.Date,
                opt => opt.MapFrom(origin => origin.Notification.Date.ToString(DateConstants.DateTimeFormat))
                )
                .ForMember(
                destiny => destiny.Description,
                opt => opt.MapFrom(origin => origin.Notification.Description)
                )
                .ForMember(
                destiny => destiny.SubscriptionId,
                opt => opt.MapFrom(origin => origin.Notification.SubscriptionId)
                );

            CreateMap<NotificationViewModel, Notification>()
                .ForMember(
                destiny => destiny.Date,
                opt => opt.MapFrom(origin => DateTime.ParseExact(origin.Date, DateConstants.DateTimeFormat, CultureInfo.InvariantCulture))
                );

            CreateMap<Subscription, SubscriptionViewModel>()
                .ForMember(
                destiny => destiny.StartDate,
                opt => opt.MapFrom(origin => origin.StartDate.ToString(DateConstants.DateOnlyFormat))
                )
                .ForMember(
                destiny => destiny.EndDate,
                opt => opt.MapFrom(origin => origin.EndDate.ToString(DateConstants.DateOnlyFormat))
                )
                .ForMember(
                destiny => destiny.CreatedOn,
                opt => opt.MapFrom(origin => origin.CreatedOn.ToString(DateConstants.DateTimeFormat))
                )
                .ForMember(
                destiny => destiny.UpdatedOn,
                opt => opt.MapFrom(origin => origin.UpdatedOn.ToString(DateConstants.DateTimeFormat))
                )
                .ForMember(
                destiny => destiny.ClientName,
                opt => opt.MapFrom(origin => origin.Client!.Name)
                )
                .ForMember(
                destiny => destiny.ServiceName,
                opt => opt.MapFrom(origin => origin.Service.Name)
                )
                .ForMember(
                destiny => destiny.CreatedByCode,
                opt => opt.MapFrom(origin => origin.CreatedBy!.Code)
                )
                .ForMember(
                destiny => destiny.CreatedByName,
                opt => opt.MapFrom(origin => origin.CreatedBy!.FirstName + " " + origin.CreatedBy.LastName)
                )
                .ForMember(
                destiny => destiny.UpdatedByCode,
                opt => opt.MapFrom(origin => origin.UpdatedBy!.Code)
                )
                .ForMember(
                destiny => destiny.UpdatedByName,
                opt => opt.MapFrom(origin => origin.UpdatedBy!.FirstName + " " + origin.UpdatedBy.LastName)
                )
                .ForMember(
                destiny => destiny.ClientRecipients,
                opt => opt.MapFrom(origin => origin.SubscriptionClients.Select(p => p.Client).ToList())
                )
                .ForMember(
                destiny => destiny.UserRecipients,
                opt => opt.MapFrom(origin => origin.SubscriptionUsers.Select(p => p.User).ToList())
                )
                .ForMember(
                destiny => destiny.RemainingDays,
                opt => opt.MapFrom(origin => (origin.EndDate.Date - DateTime.Now.Date).Days)
                );

            CreateMap<SubscriptionViewModel, Subscription>()
                .ForMember(
                destiny => destiny.StartDate,
                opt => opt.MapFrom(origin => DateTime.ParseExact(string.IsNullOrEmpty(origin.StartDate) ? DateConstants.DateOnlyDefault : origin.StartDate, DateConstants.DateOnlyFormat, CultureInfo.InvariantCulture))
                )
                .ForMember(
                destiny => destiny.EndDate,
                opt => opt.MapFrom(origin => DateTime.ParseExact(string.IsNullOrEmpty(origin.EndDate) ? DateConstants.DateOnlyDefault : origin.EndDate, DateConstants.DateOnlyFormat, CultureInfo.InvariantCulture))
                )
                .ForMember(
                destiny => destiny.CreatedOn,
                opt => opt.MapFrom(origin => DateTime.ParseExact(string.IsNullOrEmpty(origin.CreatedOn) ? DateConstants.DateTimeDefault : origin.CreatedOn, DateConstants.DateTimeFormat, CultureInfo.InvariantCulture))
                )
                .ForMember(
                destiny => destiny.UpdatedOn,
                opt => opt.MapFrom(origin => DateTime.ParseExact(string.IsNullOrEmpty(origin.UpdatedOn) ? DateConstants.DateTimeDefault : origin.UpdatedOn, DateConstants.DateTimeFormat, CultureInfo.InvariantCulture))
                );

        }
    }
}
