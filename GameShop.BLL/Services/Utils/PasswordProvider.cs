using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Services.Interfaces.Utils;

namespace GameShop.BLL.Services.Utils
{
    public class PasswordProvider : IPasswordProvider
    {
        public string GetPasswordHash(string password)
        {
            byte[] salt = GenerateSalt();
            byte[] hash = GenerateHash(password, salt);
            string saltString = Convert.ToBase64String(salt);
            string hashString = Convert.ToBase64String(hash);
            return $"{saltString}:{hashString}";
        }

        public bool IsPasswordValid(string password, string storedPasswordHash)
        {
            string[] parts = storedPasswordHash.Split(':');
            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid stored password hash format");
            }

            string saltString = parts[0];
            string hashString = parts[1];

            byte[] salt = Convert.FromBase64String(saltString);
            byte[] hash = Convert.FromBase64String(hashString);

            byte[] inputHash = GenerateHash(password, salt);

            return AreHashesEqual(hash, inputHash);
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        private byte[] GenerateHash(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                return pbkdf2.GetBytes(32);
            }
        }

        private bool AreHashesEqual(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
            {
                return false;
            }

            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
