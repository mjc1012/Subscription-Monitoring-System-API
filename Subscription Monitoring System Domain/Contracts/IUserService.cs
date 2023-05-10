using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IUserService
    {
        Task<List<UserViewModel>> GetActiveList();
        Task<UserViewModel> GetActive(int id);
        Task<UserViewModel> GetInactive(int id);
        Task<UserViewModel> GetActive(string code);
        Task<User> GetByEmail(string emailAddress);
        Task<User> Get(string name);
        List<UserViewModel> SortAscending(string sortBy, List<UserViewModel> subscriptions);
        List<UserViewModel> SortDescending(string sortBy, List<UserViewModel> subscriptions);
        Task<ListViewModel> GetList(UserFilterViewModel filter);
        Task<List<UserViewModel>> GetList(List<int> ids);
        Task Create(UserViewModel user, DepartmentViewModel department);
        Task Update(UserViewModel newUser, UserViewModel oldUser, DepartmentViewModel department);
        Task SoftDelete(int id);
        Task HardDelete(int id);
        Task SoftDelete(RecordIdsViewModel records);
        Task HardDelete(RecordIdsViewModel records);
        Task Restore(int id);
        Task Restore(RecordIdsViewModel records);
        Task<bool> UserExists(UserViewModel user);
        Task<bool> RefreshTokenExists(string refreshToken);
    }
}
