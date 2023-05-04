using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Subscription_Monitoring_System_Data;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using Subscription_Monitoring_System_Domain.Services;
using System.Data;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationAPIController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationViewModel loginUser)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.AuthenticationHandler.VerifyUser(loginUser);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }

                string accessToken = _unitOfWork.AuthenticationService.CreateJwt(loginUser.Code);
                string refreshToken = await _unitOfWork.AuthenticationService.CreateRefreshToken();
                DateTime refreshTokenExpiryTime = DateTime.Now.AddDays(5);
                await _unitOfWork.AuthenticationService.SaveTokens(loginUser, accessToken, refreshToken, refreshTokenExpiryTime);
                TokenViewModel token = new()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = AuthenticationConstants.LoggedIn, Value = token });
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordViewModel user)
        {
            try
            {

                List<string> validationErrors = await _unitOfWork.AuthenticationHandler.CanChangePassword(user);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                AuthenticationViewModel authentication = new()
                {
                    Code = user.Code,
                    Password = user.NewPassword
                };
                await _unitOfWork.AuthenticationService.ChangePassword(authentication);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = AuthenticationConstants.PasswordChanged});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenViewModel token)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.AuthenticationHandler.CanRefreshToken(token);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }

                ClaimsPrincipal principal = _unitOfWork.AuthenticationService.GetPrincipalFromExpiredToken(token.AccessToken);
                User user = await _unitOfWork.UserService.Get(principal.Identity.Name);

                string newAccessToken = _unitOfWork.AuthenticationService.CreateJwt(user.Code);
                string newRefreshToken = await _unitOfWork.AuthenticationService.CreateRefreshToken();
                await _unitOfWork.AuthenticationService.SaveTokens(user, newAccessToken, newRefreshToken);
                TokenViewModel newTokens = new()
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                };

                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = "Token refreshed", Value = newTokens });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost("send-reset-email/{emailAddress}")]
        public async Task<IActionResult> ResetPasswordEmail(string emailAddress)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.UserHandler.CanEmail(emailAddress);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }

                User user = await _unitOfWork.UserService.GetByEmail(emailAddress);

                byte[] tokenBytes = RandomNumberGenerator.GetBytes(64);
                string resetPasswordToken = Convert.ToBase64String(tokenBytes);
                DateTime resetPasswordExpiry = DateTime.Now.AddMinutes(15);
                await _unitOfWork.AuthenticationService.SaveTokens(user, resetPasswordToken, resetPasswordExpiry);
                _unitOfWork.EmailService.SendEmail(new EmailViewModel(emailAddress, "Reset Password", EmailBody.ResetEmailBody(emailAddress, resetPasswordToken)));

                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = EmailConstants.ResetPasswordEmailed });
                

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPasssword([FromBody] ResetPasswordViewModel resetPassword)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.AuthenticationHandler.CanResetPassword(resetPassword);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }

                User user = await _unitOfWork.UserService.GetByEmail(resetPassword.EmailAddress);
                AuthenticationViewModel authentication = new()
                {
                    Code = user.Code,
                    Password = resetPassword.NewPassword
                };
                await _unitOfWork.AuthenticationService.ChangePassword(authentication);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = "Password Reset Successful" });
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }
    }
}
