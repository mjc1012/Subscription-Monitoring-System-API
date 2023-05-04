using Subscription_Monitoring_System_Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IUserHandler
    {

        Task<List<string>> CanAdd(UserViewModel user);
        Task<List<string>> CanUpdate(UserViewModel user);
        Task<List<string>> CanDeleteActive(int id);
        Task<List<string>> CanDeleteInactive(int id);
        Task<List<string>> CanEmail(string email);
        Task<List<string>> CanDelete(RecordIdsViewModel records);
        Task<List<string>> CanRestore(int id);
        Task<List<string>> CanRestore(RecordIdsViewModel records);
    }
}
