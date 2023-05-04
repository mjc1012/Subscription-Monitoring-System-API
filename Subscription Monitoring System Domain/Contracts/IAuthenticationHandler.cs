using Subscription_Monitoring_System_Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IAuthenticationHandler
    {

        Task<List<string>> VerifyUser(AuthenticationViewModel loginUser);
        Task<List<string>> CanChangePassword(UpdatePasswordViewModel user);
        Task<List<string>> CanRefreshToken(TokenViewModel token);
        Task<List<string>> CanResetPassword(ResetPasswordViewModel resetPassword);
    }
}
