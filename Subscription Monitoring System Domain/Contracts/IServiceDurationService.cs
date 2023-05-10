using Subscription_Monitoring_System_Data.ViewModels;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IServiceDurationService
    {

        Task<ServiceDurationViewModel> Get(int id);
        Task<ServiceDurationViewModel> Get(string name);
        Task<List<ServiceDurationViewModel>> GetList();
        Task<List<ServiceDurationViewModel>> GetList(List<int> ids);
        Task Create(ServiceDurationViewModel serviceDuration);
        Task Update(ServiceDurationViewModel serviceDuration);
        Task HardDelete(int id);
        Task HardDelete(RecordIdsViewModel records);
        Task<bool> ServiceDurationExists(ServiceDurationViewModel serviceDuration);
    }
}
