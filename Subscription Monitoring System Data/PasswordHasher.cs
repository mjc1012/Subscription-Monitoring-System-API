using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Data
{
    public class PasswordHasher
    {
        private static readonly RNGCryptoServiceProvider rng = new();
        
        public static string HashPassword(string password)
        {
            byte[] salt;
            rng.GetBytes(salt = new byte[PasswordConstants.SaltSize]);
            var key = new Rfc2898DeriveBytes(password, salt, PasswordConstants.Iterations);
            var hash = key.GetBytes(PasswordConstants.HashSize);
            var hashBytes = new byte[PasswordConstants.SaltSize + PasswordConstants.HashSize];
            Array.Copy(salt, 0, hashBytes, 0, PasswordConstants.SaltSize);
            Array.Copy(hash, 0, hashBytes, PasswordConstants.SaltSize, PasswordConstants.HashSize);
            var base64Hash = Convert.ToBase64String(hashBytes);
            return base64Hash;
        }
    }
}
