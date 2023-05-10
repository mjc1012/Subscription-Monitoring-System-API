using Subscription_Monitoring_System_Data.ViewModels;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IServiceService
    {
        Task<List<ServiceViewModel>> GetActiveList();
        Task<ServiceViewModel> GetActive(int id);
        Task<ServiceViewModel> GetInactive(int id);
        Task<ServiceViewModel> GetActive(string name);
        List<ServiceViewModel> SortAscending(string sortBy, List<ServiceViewModel> services);
        List<ServiceViewModel> SortDescending(string sortBy, List<ServiceViewModel> services);
        Task<ListViewModel> GetList(ServiceFilterViewModel filter);
        Task<List<ServiceViewModel>> GetList(List<int> ids);
        Task Create(ServiceViewModel service, ServiceDurationViewModel serviceDuration);
        Task Update(ServiceViewModel service, ServiceDurationViewModel serviceDuration);
        Task SoftDelete(int id);
        Task HardDelete(int id);
        Task SoftDelete(RecordIdsViewModel records);
        Task HardDelete(RecordIdsViewModel records);
        Task Restore(int id);
        Task Restore(RecordIdsViewModel records);
        Task<bool> ServiceExists(ServiceViewModel service);
    }
}
