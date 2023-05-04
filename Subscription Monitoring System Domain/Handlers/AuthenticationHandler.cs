using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Subscription_Monitoring_System_Data.Constants;
using IAuthenticationHandler = Subscription_Monitoring_System_Domain.Contracts.IAuthenticationHandler;
using IAuthenticationService = Subscription_Monitoring_System_Domain.Contracts.IAuthenticationService;

namespace Subscription_Monitoring_System_Domain.Handlers
{
    public class AuthenticationHandler : IAuthenticationHandler
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationHandler(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }
        public async Task<List<string>> VerifyUser(AuthenticationViewModel loginUser)
        {
            var validationErrors = new List<string>();

            if (loginUser != null)
            {
                if (!await _authenticationService.UserExists(loginUser))
                {
                    validationErrors.Add(UserConstants.DoesNotExist);
                }

                if (!await _authenticationService.VerifyPassword(loginUser))
                {
                    validationErrors.Add(AuthenticationConstants.PasswordInvalid);
                }
            }
            else
            {
                validationErrors.Add(UserConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanChangePassword(UpdatePasswordViewModel user)
        {
            var validationErrors = new List<string>();

            if (user == null)
            {
                validationErrors.Add(PasswordConstants.EntryInvalid);
            }
            else
            {
                AuthenticationViewModel authentication = new()
                {
                    Code = user.Code,
                    Password = user.OldPassword
                };
                 if (!await _authenticationService.VerifyPassword(authentication))
                {
                    validationErrors.Add(AuthenticationConstants.PasswordInvalid);
                }
                if (user.NewPassword.Length < 8)
                {
                    validationErrors.Add(PasswordConstants.PasswordLengthError);
                }
                if (!(Regex.IsMatch(user.NewPassword, "[~,',!,@,#,$,%,^,&,*,(,),-,_,+,=,{,},\\[,\\],|,/,\\,:,;,\",`,<,>,,,.,?]") && Regex.IsMatch(user.NewPassword, "[a-z]") && Regex.IsMatch(user.NewPassword, "[A-Z]") && Regex.IsMatch(user.NewPassword, "[0-9]")))
                {
                    validationErrors.Add(PasswordConstants.PasswordCharacterError);
                }
                if (!await _authenticationService.UserExists(authentication))
                {
                    validationErrors.Add(UserConstants.DoesNotExist);
                }
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRefreshToken(TokenViewModel token)
        {
            var validationErrors = new List<string>();

            if (token != null)
            {
                ClaimsPrincipal principal = _authenticationService.GetPrincipalFromExpiredToken(token.AccessToken);
                User user = await _userService.Get(principal.Identity.Name);
                if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    validationErrors.Add(AuthenticationConstants.TokenInvalid);
                }
            }
            else
            {
                validationErrors.Add(AuthenticationConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanResetPassword(ResetPasswordViewModel resetPassword)
        {
            var validationErrors = new List<string>();

            if (resetPassword == null)
            {
                validationErrors.Add(PasswordConstants.EntryInvalid);
            }
            else
            {
                if (resetPassword.NewPassword.Length < 8)
                {
                    validationErrors.Add(PasswordConstants.PasswordLengthError);
                }
                if (!(Regex.IsMatch(resetPassword.NewPassword, "[~,',!,@,#,$,%,^,&,*,(,),-,_,+,=,{,},\\[,\\],|,/,\\,:,;,\",`,<,>,,,.,?]") && Regex.IsMatch(resetPassword.NewPassword, "[a-z]") && Regex.IsMatch(resetPassword.NewPassword, "[A-Z]") && Regex.IsMatch(resetPassword.NewPassword, "[0-9]")))
                {
                    validationErrors.Add(PasswordConstants.PasswordCharacterError);
                }
                string newToken = resetPassword.ResetPasswordToken.Replace(" ", "+");

                User user = await _userService.GetByEmail(resetPassword.EmailAddress);

                if (user == null)
                {
                    validationErrors.Add(UserConstants.EmailAddressDoesNotExist);
                }
                else if (user.ResetPasswordToken != newToken || user.ResetPasswordExpiry < DateTime.Now)
                {
                    validationErrors.Add(PasswordConstants.RefreshTokenError);
                }
            }

            return validationErrors;
        }
    }
}
