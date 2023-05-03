using Subscription_Monitoring_System_Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IServiceService
    {
        Task<List<ServiceDto>> GetActiveList();
        Task<ServiceDto> GetActive(int id);
        Task<ServiceDto> GetInactive(int id);
        Task<ServiceDto> GetActive(string name);
        Task<ListDto> GetList(ServiceFilterDto filter);
        Task<List<ServiceDto>> GetList(List<int> ids);
        Task Create(ServiceDto service, ServiceTypeDto serviceType);
        Task Update(ServiceDto service, ServiceTypeDto serviceType);
        Task SoftDelete(int id);
        Task HardDelete(int id);
        Task SoftDelete(RecordIdsDto records);
        Task HardDelete(RecordIdsDto records);
        Task Restore(int id);
        Task Restore(RecordIdsDto records);
        Task<bool> ServiceExists(ServiceDto service);
    }
}
