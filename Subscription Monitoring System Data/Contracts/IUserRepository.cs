using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Contracts
{
    public interface IUserRepository
    {
        Task<List<User>> GetActiveList();
        Task<User?> GetActive(int id);
        Task<User?> GetInactive(int id);
        Task<User?> GetActive(string code);
        Task<List<User>> GetList();
        Task<List<User>> GetList(List<int> ids);
        Task Create(User user);
        Task Update(User newUser, User oldUser);
        Task ChangePassword(User user);
        Task SoftDelete(int id);
        Task HardDelete(int id);
        Task SoftDelete(List<int> ids);
        Task HardDelete(List<int> ids);
        Task Restore(int id);
        Task Restore(List<int> ids);
        Task<bool> UserExists(User user);
        Task<bool> RefreshTokenExists(string refreshToken);
        Task<User?> GetActiveByEmail(string emailAddress);
        Task SaveTokens(User user);
    }
}
