using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Handlers
{
    public class UserNotificationHandler : IUserNotificationHandler
    {
        private readonly IUserNotificationService _userNotificationService;
        public UserNotificationHandler(IUserNotificationService userNotificationService)
        {
            _userNotificationService = userNotificationService;
        }

        public async Task<List<string>> CanDeleteActive(int id)
        {
            var validationErrors = new List<string>();

            UserNotification userNotification = await _userNotificationService.GetActive(id);
            if (userNotification == null)
            {
                validationErrors.Add(NotificationConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteInactive(int id)
        {
            var validationErrors = new List<string>();

            UserNotification userNotification = await _userNotificationService.GetInactive(id);
            if (userNotification == null)
            {
                validationErrors.Add(NotificationConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(RecordIdsViewModel records)
        {
            var validationErrors = new List<string>();

            List<UserNotification> userNotifications = await _userNotificationService.GetList(records.Ids);
            if (userNotifications == null)
            {
                validationErrors.Add(NotificationConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(int id)
        {
            var validationErrors = new List<string>();

            UserNotification userNotification = await _userNotificationService.GetInactive(id);
            if (userNotification == null)
            {
                validationErrors.Add(NotificationConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(RecordIdsViewModel records)
        {
            var validationErrors = new List<string>();

            List<UserNotification> userNotifications = await _userNotificationService.GetList(records.Ids);
            if (userNotifications == null)
            {
                validationErrors.Add(NotificationConstants.DoesNotExist);
            }

            return validationErrors;
        }

    }
}
