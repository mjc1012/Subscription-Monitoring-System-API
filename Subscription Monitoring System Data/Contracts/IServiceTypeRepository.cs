using Subscription_Monitoring_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Contracts
{
    public interface IServiceTypeRepository
    {
        Task<ServiceType?> Get(int id);
        Task<ServiceType?> Get(string name);
        Task<List<ServiceType>> GetList();
        Task<bool> ServiceTypeExists(string name);
    }
}
