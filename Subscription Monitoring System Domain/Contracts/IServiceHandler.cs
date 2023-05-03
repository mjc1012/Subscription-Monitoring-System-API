using Subscription_Monitoring_System_Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IServiceHandler
    {
        Task<List<string>> CanAdd(ServiceDto service);
        Task<List<string>> CanUpdate(ServiceDto service);
        Task<List<string>> CanDeleteActive(int id);
        Task<List<string>> CanDeleteInactive(int id);
        Task<List<string>> CanDelete(RecordIdsDto records);
        Task<List<string>> CanRestore(int id);
        Task<List<string>> CanRestore(RecordIdsDto records);
    }
}
