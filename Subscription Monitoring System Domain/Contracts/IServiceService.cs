using Subscription_Monitoring_System_Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IServiceService
    {
        Task<List<ServiceViewModel>> GetActiveList();
        Task<ServiceViewModel> GetActive(int id);
        Task<ServiceViewModel> GetInactive(int id);
        Task<ServiceViewModel> GetActive(string name);
        Task<ListViewModel> GetList(ServiceFilterViewModel filter);
        Task<List<ServiceViewModel>> GetList(List<int> ids);
        Task Create(ServiceViewModel service, ServiceTypeViewModel serviceType);
        Task Update(ServiceViewModel service, ServiceTypeViewModel serviceType);
        Task SoftDelete(int id);
        Task HardDelete(int id);
        Task SoftDelete(RecordIdsViewModel records);
        Task HardDelete(RecordIdsViewModel records);
        Task Restore(int id);
        Task Restore(RecordIdsViewModel records);
        Task<bool> ServiceExists(ServiceViewModel service);
    }
}
