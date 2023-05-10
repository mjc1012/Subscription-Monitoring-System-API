using System.Security.Cryptography;
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
            Rfc2898DeriveBytes key = new(password, salt, PasswordConstants.Iterations);
            byte[] hash = key.GetBytes(PasswordConstants.HashSize);
            byte[] hashBytes = new byte[PasswordConstants.SaltSize + PasswordConstants.HashSize];
            Array.Copy(salt, 0, hashBytes, 0, PasswordConstants.SaltSize);
            Array.Copy(hash, 0, hashBytes, PasswordConstants.SaltSize, PasswordConstants.HashSize);
            string base64Hash = Convert.ToBase64String(hashBytes);
            return base64Hash;
        }
    }
}
