using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Data.Models;
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
        Task<bool> VerifyPassword(AuthenticationDto loginUser);
        Task<bool> UserExists(AuthenticationDto loginUser);
        Task ChangePassword(AuthenticationDto user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken);

        Task<string> CreateRefreshToken();

        Task SaveTokens(User user, string accessToken, string refreshToken);


        Task SaveTokens(User user, string refreshPasswordToken, DateTime resetPasswordExpiry);

        Task SaveTokens(AuthenticationDto user, string accessToken, string refreshToken, DateTime refreshTokenExpiryTime);

        string CreateJwt(string code);
    }
}
