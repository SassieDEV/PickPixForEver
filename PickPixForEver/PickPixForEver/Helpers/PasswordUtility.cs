using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PickPixForEver.Helpers
{
    public static class PasswordUtility
    {
        public static KeyValuePair<string, string> EncryptPassword(string password, string passwordSalt = null)
        {
            string passwordHash = null;
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            byte[] saltBytes = null;

            if (!string.IsNullOrEmpty(passwordSalt))
            {
                saltBytes = Convert.FromBase64String(passwordSalt);
            }
            else
            {
                saltBytes = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(saltBytes);
                }
            }

            byte[] totalBytes = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, totalBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, totalBytes, saltBytes.Length, passwordBytes.Length);

            using (SHA256 hashAlgorithm = SHA256.Create())
            {
                passwordHash = Convert.ToBase64String(hashAlgorithm.ComputeHash(totalBytes));
            }

            passwordSalt = Convert.ToBase64String(saltBytes);
            return new KeyValuePair<string, string>(passwordHash, passwordSalt);
        }
    }
}
