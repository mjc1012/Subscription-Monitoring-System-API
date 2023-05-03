using Subscription_Monitoring_System_Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IAuthenticationHandler
    {

        Task<List<string>> VerifyUser(AuthenticationDto loginUser);
        Task<List<string>> CanChangePassword(UpdatePasswordDto user);
        Task<List<string>> CanRefreshToken(TokenDto token);
        Task<List<string>> CanResetPassword(ResetPasswordDto resetPassword);
    }
}
