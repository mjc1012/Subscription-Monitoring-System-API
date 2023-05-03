using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IClientService
    {
        Task<List<ClientDto>> GetActiveList();
        Task<ClientDto> GetActive(int id);
        Task<ClientDto> GetInactive(int id);
        Task<ClientDto> GetActive(string name);
        Task<ListDto> GetList(ClientFilterDto filter);
        Task<List<ClientDto>> GetList(List<int> ids);
        Task Create(ClientDto client);
        Task Update(ClientDto client);
        Task SoftDelete(int id);
        Task HardDelete(int id);
        Task SoftDelete(RecordIdsDto records);
        Task HardDelete(RecordIdsDto records);
        Task Restore(int id);
        Task Restore(RecordIdsDto records);
        Task<bool> ClientExists(ClientDto client);
    }
}
