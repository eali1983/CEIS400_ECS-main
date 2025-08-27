using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CEIS400_ECS
{
    public static class PasswordControl
    {
        public static string HashPassword(string password, out string saltBase64)
        {
            using (var hmac = new HMACSHA512())
            {
                byte[] salt = hmac.Key;
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                saltBase64 = Convert.ToBase64String(salt);
                return Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] salt = Convert.FromBase64String(storedSalt);
            using (var hmac = new HMACSHA512(salt))
            {
                byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                string computedHashBase64 = Convert.ToBase64String(computedHash);
                return computedHashBase64 == storedHash;
            }
        }
    }
}
