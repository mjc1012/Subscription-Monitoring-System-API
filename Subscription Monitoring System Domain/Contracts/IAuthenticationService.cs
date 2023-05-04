using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IAuthenticationService
    {
        Task<bool> VerifyPassword(AuthenticationViewModel loginUser);
        Task<bool> UserExists(AuthenticationViewModel loginUser);
        Task ChangePassword(AuthenticationViewModel user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken);

        Task<string> CreateRefreshToken();

        Task SaveTokens(User user, string accessToken, string refreshToken);


        Task SaveTokens(User user, string refreshPasswordToken, DateTime resetPasswordExpiry);

        Task SaveTokens(AuthenticationViewModel user, string accessToken, string refreshToken, DateTime refreshTokenExpiryTime);

        string CreateJwt(string code);
    }
}
