using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<List<string>> CanDelete(int id)
        {
            var validationErrors = new List<string>();

            NotificationDto client = await _notificationService.Get(id);
            if (client == null)
            {
                validationErrors.Add(NotificationConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(RecordIdsDto records)
        {
            var validationErrors = new List<string>();

            List<NotificationDto> clients = await _notificationService.GetList(records.Ids);
            if (clients == null)
            {
                validationErrors.Add(NotificationConstants.DoesNotExist);
            }

            return validationErrors;
        }
    }
}
