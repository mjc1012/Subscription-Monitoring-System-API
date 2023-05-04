using Subscription_Monitoring_System_Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface INotificationHandler
    {
        Task<List<string>> CanDelete(int id);
        Task<List<string>> CanDelete(RecordIdsViewModel records);
    }
}
