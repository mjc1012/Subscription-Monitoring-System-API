using Subscription_Monitoring_System_Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IDepartmentService
    {
        Task<DepartmentDto> Get(int id);
        Task<DepartmentDto> Get(string name);
        Task<List<DepartmentDto>> GetList();
    }
}
