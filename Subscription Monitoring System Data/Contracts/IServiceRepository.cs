using Subscription_Monitoring_System_Data.Models;

namespace Subscription_Monitoring_System_Data.Contracts
{
    public interface IServiceRepository
    {
        Task<List<Service>> GetActiveList();
        Task<Service?> GetActive(int id);
        Task<Service?> GetInactive(int id);
        Task<Service?> GetActive(string name);
        Task<List<Service>> GetList();
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
