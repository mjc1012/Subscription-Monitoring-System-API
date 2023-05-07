using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Subscription_Monitoring_System_Data;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public AuthenticationService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> UserExists(AuthenticationViewModel user)
        {
            try
            {
                return await _userRepository.UserExists(_mapper.Map<User>(user));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> VerifyPassword(AuthenticationViewModel loginUser)
        {
            User dbUser = await _userRepository.GetActive(loginUser.Code);
            var hashBytes = Convert.FromBase64String(dbUser.Password);
            var salt = new byte[PasswordConstants.SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, PasswordConstants.SaltSize);
            var key = new Rfc2898DeriveBytes(loginUser.Password, salt, PasswordConstants.Iterations);
            byte[] hash = key.GetBytes(PasswordConstants.HashSize);

            for (int i = 0; i < PasswordConstants.HashSize; i++)
            {
                if (hashBytes[i + PasswordConstants.SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }

        public string CreateJwt(string code)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthenticationConstants.SecretKey);
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, code)
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = credentials,
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken)
        {
            var key = Encoding.ASCII.GetBytes(AuthenticationConstants.SecretKey);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(expiredToken, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("This is an invalid token");
            }
            else
            {
                return principal;
            }
        }



        public async Task<string> CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes).Replace("/", "!").Replace("\\", "!");

            if (await _userRepository.RefreshTokenExists(refreshToken))
            {
                return await CreateRefreshToken();
            }
            return refreshToken;
        }

        public async Task SaveTokens(User user,string accessToken, string refreshToken)
        {
            try
            {
                user.AccessToken = accessToken;
                user.RefreshToken = refreshToken;

                await _userRepository.SaveTokens(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveTokens(AuthenticationViewModel user, string accessToken, string refreshToken, DateTime refreshTokenExpiryTime)
        {
            try
            {
                User retrievedUser = await _userRepository.GetActive(user.Code);
                retrievedUser.AccessToken = accessToken;
                retrievedUser.RefreshToken = refreshToken;
                retrievedUser.RefreshTokenExpiryTime = refreshTokenExpiryTime;
                await _userRepository.SaveTokens(retrievedUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveTokens(User user, string refreshPasswordToken, DateTime resetPasswordExpiry)
        {
            try
            {
                user.ResetPasswordToken = refreshPasswordToken;
                user.ResetPasswordExpiry = resetPasswordExpiry;
                await _userRepository.SaveTokens(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ChangePassword(AuthenticationViewModel user)
        {
            try
            {
                User userMapped = new()
                {
                    Code = user.Code,
                    Password = PasswordHasher.HashPassword(user.Password)
                };
                await _userRepository.ChangePassword(userMapped);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
