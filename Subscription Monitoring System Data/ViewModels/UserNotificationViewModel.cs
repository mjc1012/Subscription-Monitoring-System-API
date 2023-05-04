using Subscription_Monitoring_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.ViewModels
{
    public class UserNotificationViewModel
    {
        public List<NotificationViewModel> Notifications { get; set; } = new List<NotificationViewModel>();
    }
}
