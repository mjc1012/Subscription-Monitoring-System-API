using AutoMapper;
using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Client, ClientDto>().ReverseMap();

            CreateMap<Department, DepartmentDto>().ReverseMap();

            CreateMap<ServiceTypeDto, ServiceType>().ReverseMap();

            CreateMap<ServiceDto, Service>();

            CreateMap<Service, ServiceDto>()
                .ForMember(
                destiny => destiny.ServiceTypeName,
                opt => opt.MapFrom(origin => origin.ServiceType.Name)
                );

            CreateMap<User, UserDto>()
                 .ForMember(
                destiny => destiny.DepartmentName,
                opt => opt.MapFrom(origin => origin.Department.Name)
                )
                .ForMember(
                destiny => destiny.UnseenNotifications,
                opt => opt.MapFrom(origin => origin.UserNotifications.Where(p=> p.IsSeen == false).Count())
                );

            CreateMap<UserDto, User>();

            CreateMap<AuthenticationDto, User>();

            CreateMap<Notification, NotificationDto>()
                .ForMember(
                destiny => destiny.Date,
                opt => opt.MapFrom(origin => origin.Date.ToString("yyyy-MM-dd HH:mm:ss"))
                );

            CreateMap<NotificationDto, Notification>()
                .ForMember(
                destiny => destiny.Date,
                opt => opt.MapFrom(origin => DateTime.ParseExact(origin.Date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))
                );

            CreateMap<Subscription, SubscriptionDto>()
                .ForMember(
                destiny => destiny.StartDate,
                opt => opt.MapFrom(origin => origin.StartDate.ToString("yyyy-MM-dd"))
                )
                .ForMember(
                destiny => destiny.EndDate,
                opt => opt.MapFrom(origin => origin.EndDate.ToString("yyyy-MM-dd"))
                )
                .ForMember(
                destiny => destiny.CreatedOn,
                opt => opt.MapFrom(origin => origin.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss"))
                )
                .ForMember(
                destiny => destiny.UpdatedOn,
                opt => opt.MapFrom(origin => origin.UpdatedOn.ToString("yyyy-MM-dd HH:mm:ss"))
                )
                .ForMember(
                destiny => destiny.ClientName,
                opt => opt.MapFrom(origin => origin.Client.Name)
                )
                .ForMember(
                destiny => destiny.ServiceName,
                opt => opt.MapFrom(origin => origin.Service.Name)
                )
                .ForMember(
                destiny => destiny.CreatedByCode,
                opt => opt.MapFrom(origin => origin.CreatedBy.Code)
                )
                .ForMember(
                destiny => destiny.CreatedByName,
                opt => opt.MapFrom(origin => origin.CreatedBy.FirstName + " " + origin.CreatedBy.LastName)
                )
                .ForMember(
                destiny => destiny.UpdatedByCode,
                opt => opt.MapFrom(origin => origin.UpdatedBy.Code)
                )
                .ForMember(
                destiny => destiny.UpdatedByName,
                opt => opt.MapFrom(origin => origin.UpdatedBy.FirstName + " " + origin.UpdatedBy.LastName)
                )
                .ForMember(
                destiny => destiny.ClientRecipients,
                opt => opt.MapFrom(origin => origin.SubscriptionClients.Select(p => p.Client).ToList())
                )
                .ForMember(
                destiny => destiny.UserRecipients,
                opt => opt.MapFrom(origin => origin.SubscriptionUsers.Select(p => p.User).ToList())
                );

            CreateMap<SubscriptionDto, Subscription>()
                .ForMember(
                destiny => destiny.StartDate,
                opt => opt.MapFrom(origin => DateTime.ParseExact(string.IsNullOrEmpty(origin.StartDate) ? "0001-01-01" : origin.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                )
                .ForMember(
                destiny => destiny.EndDate,
                opt => opt.MapFrom(origin => DateTime.ParseExact(string.IsNullOrEmpty(origin.EndDate) ? "0001-01-01" : origin.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                )
                .ForMember(
                destiny => destiny.CreatedOn,
                opt => opt.MapFrom(origin => DateTime.ParseExact(string.IsNullOrEmpty(origin.CreatedOn) ? "0001-01-01 00:00:00" : origin.CreatedOn, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))
                )
                .ForMember(
                destiny => destiny.UpdatedOn,
                opt => opt.MapFrom(origin => DateTime.ParseExact(string.IsNullOrEmpty(origin.UpdatedOn) ? "0001-01-01 00:00:00" : origin.UpdatedOn, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))
                );

        }
    }
}
