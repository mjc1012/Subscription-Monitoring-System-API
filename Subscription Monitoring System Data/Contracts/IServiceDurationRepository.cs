using Subscription_Monitoring_System_Data.Models;

namespace Subscription_Monitoring_System_Data.Contracts
{
    public interface IServiceDurationRepository
    {
        Task<ServiceDuration?> Get(int id);
        Task<ServiceDuration?> Get(string name);
        Task<List<ServiceDuration>> GetList();
        Task<List<ServiceDuration>> GetList(List<int> ids);
        Task Create(ServiceDuration serviceDuration);
        Task Update(ServiceDuration serviceDuration);
        Task HardDelete(int id);
        Task HardDelete(List<int> ids);
        Task<bool> ServiceDurationExists(ServiceDuration serviceDuration);
    }
}
