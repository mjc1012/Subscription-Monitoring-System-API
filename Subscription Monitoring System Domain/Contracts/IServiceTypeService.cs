using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IServiceTypeService
    {

        Task<ServiceTypeViewModel> Get(int id);
        Task<ServiceTypeViewModel> Get(string name);
        Task<List<ServiceTypeViewModel>> GetList();
    }
}
