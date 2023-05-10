using Subscription_Monitoring_System_Data.Models;

namespace Subscription_Monitoring_System_Data.Contracts
{
    public interface IDepartmentRepository
    {
        Task<Department?> Get(int id);
        Task<Department?> Get(string name);
        Task<List<Department>> GetList();
        Task<List<Department>> GetList(List<int> ids);
        Task Create(Department department);
        Task Update(Department department);
        Task HardDelete(int id);
        Task HardDelete(List<int> ids);
        Task<bool> DepartmentExists(Department department);
    }
}
