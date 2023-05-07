using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IClientService
    {
        Task<List<ClientViewModel>> GetActiveList();
        Task<ClientViewModel> GetActive(int id);
        Task<ClientViewModel> GetInactive(int id);
        Task<ClientViewModel> GetActive(string name);
        List<ClientViewModel> SortAscending(string sortBy, List<ClientViewModel> subscriptions);
        List<ClientViewModel> SortDescending(string sortBy, List<ClientViewModel> subscriptions);
        Task<ListViewModel> GetList(ClientFilterViewModel filter);
        Task<List<ClientViewModel>> GetList(List<int> ids);
        Task Create(ClientViewModel client);
        Task Update(ClientViewModel client);
        Task SoftDelete(int id);
        Task HardDelete(int id);
        Task SoftDelete(RecordIdsViewModel records);
        Task HardDelete(RecordIdsViewModel records);
        Task Restore(int id);
        Task Restore(RecordIdsViewModel records);
        Task<bool> ClientExists(ClientViewModel client);
    }
}
