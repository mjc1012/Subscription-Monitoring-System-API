using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
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
            List<string> validationErrors = new();

            UserNotification userNotification = await _userNotificationService.GetActive(id);
            if (userNotification == null)
            {
                validationErrors.Add(NotificationConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteInactive(int id)
        {
            List<string> validationErrors = new();

            UserNotification userNotification = await _userNotificationService.GetInactive(id);
            if (userNotification == null)
            {
                validationErrors.Add(NotificationConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteActive(RecordIdsViewModel records)
        {
            List<string> validationErrors = new();

            List<UserNotification> userNotifications = await _userNotificationService.GetList(records.Ids);

            if (userNotifications.Where(p => !p.IsActive).Any() || !userNotifications.Select(p => p.Id).ToList().OrderBy(x => x).SequenceEqual(records.Ids.OrderBy(x => x)))
            {
                validationErrors.Add(NotificationConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteInactive(RecordIdsViewModel records)
        {
            List<string> validationErrors = new();

            List<UserNotification> userNotifications = await _userNotificationService.GetList(records.Ids);

            if (userNotifications.Where(p => p.IsActive).Any() || !userNotifications.Select(p => p.Id).ToList().OrderBy(x => x).SequenceEqual(records.Ids.OrderBy(x => x)))
            {
                validationErrors.Add(NotificationConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(int id)
        {
            List<string> validationErrors = new();

            UserNotification userNotification = await _userNotificationService.GetInactive(id);
            if (userNotification == null)
            {
                validationErrors.Add(NotificationConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(RecordIdsViewModel records)
        {
            List<string> validationErrors = new();

            List<UserNotification> userNotifications = await _userNotificationService.GetList(records.Ids);
            if (userNotifications == null)
            {
                validationErrors.Add(NotificationConstants.DoesNotExist);
            }
            if (userNotifications.Where(p => p.IsActive).Any())
            {
                validationErrors.Add(NotificationConstants.EntryInvalid);
            }

            return validationErrors;
        }

    }
}
