using Subscription_Monitoring_System_Data.ViewModels;

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
