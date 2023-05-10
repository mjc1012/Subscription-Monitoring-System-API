using Subscription_Monitoring_System_Data;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using Subscription_Monitoring_System_Domain.Services;
using System.Globalization;
using System.Text.RegularExpressions;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Handlers
{
    public class NotificationHandler : INotificationHandler
    {
        private readonly INotificationService _notificationService;
        public NotificationHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<List<string>> CanAdd(NotificationViewModel notification)
        {
            List<string> validationErrors = new();

            if (notification != null)
            {
                if (!DateTime.TryParseExact(notification.Date, DateConstants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    validationErrors.Add(NotificationConstants.InvalidDate);
                }

                if (await _notificationService.NotificationExists(notification))
                {
                    validationErrors.Add(NotificationConstants.Exists);
                }
            }
            else
            {
                validationErrors.Add(NotificationConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanUpdate(NotificationViewModel notification)
        {
            List<string> validationErrors = new();
            NotificationViewModel notificationFound = await _notificationService.Get(notification.Id);
            if (notification != null && notificationFound != null)
            {
                if (notification.Description == notificationFound.Description && notification.Date == notificationFound.Date && notification.SubscriptionId == notificationFound.SubscriptionId
                    && notificationFound.Users.Select(p => p.Id).ToList().Except(notification.UserIds).ToList().Any())
                {
                    validationErrors.Add(NotificationConstants.NoChanges);
                }
                else
                {
                    if (!DateTime.TryParseExact(notification.Date, DateConstants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    {
                        validationErrors.Add(NotificationConstants.InvalidDate);
                    }

                    if (await _notificationService.NotificationExists(notification))
                    {
                        validationErrors.Add(NotificationConstants.Exists);
                    }
                }
            }
            else
            {
                validationErrors.Add(NotificationConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(int id)
        {
            List<string> validationErrors = new();

            NotificationViewModel client = await _notificationService.Get(id);
            if (client == null)
            {
                validationErrors.Add(NotificationConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(RecordIdsViewModel records)
        {
            List<string> validationErrors = new();

            List<NotificationViewModel> clients = await _notificationService.GetList(records.Ids);
            if (clients == null)
            {
                validationErrors.Add(NotificationConstants.DoesNotExist);
            }

            return validationErrors;
        }
    }
}
