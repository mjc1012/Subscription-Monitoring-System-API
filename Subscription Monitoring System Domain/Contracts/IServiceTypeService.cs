using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IServiceTypeService
    {

        Task<ServiceTypeDto> Get(int id);
        Task<ServiceTypeDto> Get(string name);
        Task<List<ServiceTypeDto>> GetList();
    }
}
