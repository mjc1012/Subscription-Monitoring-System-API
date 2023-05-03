using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IUserService
    {
        Task<List<UserDto>> GetActiveList();
        Task<UserDto> GetActive(int id);
        Task<UserDto> GetInactive(int id);
        Task<UserDto> GetActive(string code);
        Task<User> GetByEmail(string emailAddress);
        Task<User> Get(string name);
        Task<ListDto> GetList(UserFilterDto filter);
        Task<List<UserDto>> GetList(List<int> ids);
        Task Create(UserDto user, DepartmentDto department);
        Task Update(UserDto user, DepartmentDto department);
        Task SoftDelete(int id);
        Task HardDelete(int id);
        Task SoftDelete(RecordIdsDto records);
        Task HardDelete(RecordIdsDto records);
        Task Restore(int id);
        Task Restore(RecordIdsDto records);
        Task<bool> UserExists(UserDto user);
        Task<bool> RefreshTokenExists(string refreshToken);
    }
}
