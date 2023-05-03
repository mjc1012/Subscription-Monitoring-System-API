using Subscription_Monitoring_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Contracts
{
    public interface IDepartmentRepository
    {
        Task<Department> Get(int id);
        Task<Department> Get(string name);
        Task<List<Department>> GetList();
    }
}
