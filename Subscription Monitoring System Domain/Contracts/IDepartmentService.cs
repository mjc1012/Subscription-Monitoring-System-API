using Subscription_Monitoring_System_Data.ViewModels;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IDepartmentService
    {
        Task<DepartmentViewModel> Get(int id);
        Task<DepartmentViewModel> Get(string name);
        Task<List<DepartmentViewModel>> GetList();
        Task<List<DepartmentViewModel>> GetList(List<int> ids);
        Task Create(DepartmentViewModel department);
        Task Update(DepartmentViewModel department);
        Task HardDelete(int id);
        Task HardDelete(RecordIdsViewModel records);
        Task<bool> DepartmentExists(DepartmentViewModel department);
    }
}
