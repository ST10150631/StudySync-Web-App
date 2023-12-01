using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudySyncClassLibrary.Classes
{
    public class PasswordHashing
    {
        public string PasswordInput { get; set; }   
        public PasswordHashing() { }

        /// <summary>
        /// Hashes and salts the password to be saved to database 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Create a new instance of the Rfc2898DeriveBytes class to perform the hash
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(32); // 32 bytes is a common choice for the hash size
                byte[] combinedBytes = new byte[salt.Length + hash.Length];

                // Combine the salt and hash bytes into a single byte array
                Array.Copy(salt, 0, combinedBytes, 0, salt.Length);
                Array.Copy(hash, 0, combinedBytes, salt.Length, hash.Length);

                // Convert the combined salt+hash bytes to a Base64-encoded string
                string base64Hash = Convert.ToBase64String(combinedBytes);

                return base64Hash;
            }
        }
        //======================================================= End of Method ===================================================


    }
}
