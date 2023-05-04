using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Contracts
{
    public interface IServiceRepository
    {
        Task<List<Service>> GetActiveList();
        Task<Service> GetActive(int id);
        Task<Service> GetInactive(int id);
        Task<Service> GetActive(string name);
        List<Service> SortAscending(string sortBy, List<Service> services);
        List<Service> SortDescending(string sortBy, List<Service> services);
        Task<List<Service>> GetList(ServiceFilterViewModel filter);
        Task<List<Service>> GetList(List<int> ids);
        Task Create(Service service);
        Task Update(Service service);
        Task SoftDelete(int id);
        Task HardDelete(int id);
        Task SoftDelete(List<int> ids);
        Task HardDelete(List<int> ids);
        Task Restore(int id);
        Task Restore(List<int> ids);
        Task<bool> ServiceExists(Service service);
    }
}
