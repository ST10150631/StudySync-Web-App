using Microsoft.Data.SqlClient;
using PROG6212_POE_PART_3_ST10150631.Models;
using StudySyncClassLibrary.Classes;
using System.Security.Cryptography;

namespace PROG6212_POE_PART_3_ST10150631.ViewModels
{
    public class UserViewModel
    {
        private PasswordHashing Hash = new PasswordHashing();
        private UserModel model = new UserModel();
        private string connectionString = "Server=tcp:dbs-vc-cldv6212-st10150631.database.windows.net,1433;Initial Catalog=PROG6212_POE_DB;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";";
        /// <summary>
        /// default constructor
        /// </summary>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public UserViewModel()
        {

        }
        //======================================================= End of Method ===================================================

        /// <summary>
        /// Takes User Input for sign Up and adds user to database if all data valid
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="FistName"></param>
        /// <param name="Surname"></param>
        /// <param name="Password"></param>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public async Task<bool> RegisterUser(string Username, string FirstName, string Surname, string Password)
        {
            if (await GetUserDetails(Username) == null)
            {
                //Hashing the password 
                var HashedPassword = Hash.HashPassword(Password);
                //Query for inserting data into the database 
                string NewUserQuery = "INSERT INTO [dbo].[User] (Username, FirstName, Surname, PasswordHash) " +
                          "VALUES (@Username, @FirstName, @Surname, @PasswordHash)";
                using (SqlConnection SQLconnect = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(NewUserQuery, SQLconnect);
                    // Add parameters to the command
                    command.Parameters.AddWithValue("@Username", Username);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@Surname", Surname);
                    command.Parameters.AddWithValue("@PasswordHash", HashedPassword);
                    // Opens the SQL connection 
                    SQLconnect.Open();
                    // Execute the query
                    await command.ExecuteNonQueryAsync();
                }
                return true;

            }
            else return false;
        }
        //======================================================= End of Method ===================================================

        /// <summary>
        /// Logs the user in and verifies password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public async Task <bool> LoginUser(string username, string password)
        {

            var user = await GetUserDetails(username);
            if (user != null)
            {
                if (VerifyPassword(password, user.PasswordHash) == true)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        //======================================================= End of Method ===================================================

        /// <summary>
        /// Verifies that the username and password match
        /// </summary>
        /// <param name="enteredPassword"></param>
        /// <param name="storedPasswordHash"></param>
        /// <returns></returns>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        private static bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            // Convert the stored password hash back to bytes
            byte[] hashBytes = Convert.FromBase64String(storedPasswordHash);

            // Extract the salt from the hash bytes
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Compute a new hash of the entered password using the stored salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000))
            {
                byte[] testHash = pbkdf2.GetBytes(32); // 32 bytes is a common choice for the hash size

                // Compare the computed hash with the stored hash
                for (int i = 0; i < 32; i++)
                {
                    if (testHash[i] != hashBytes[i + 16])
                    {
                        return false; // Passwords don't match
                    }
                }

                return true; // Passwords match
            }
        }
        //======================================================= End of Method ===================================================

        /// <summary>
        /// Retrieves user details from the database as a UserModel
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public async Task<UserModel> GetUserDetails(string username)
        {
            UserModel user = null;

            string query = "SELECT Username, FirstName, Surname, PasswordHash FROM [dbo].[User] WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();

                // Use ExecuteScalarAsync to efficiently retrieve a single row
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        // Map the data from the database to a User object
                        user = new UserModel
                        {
                            Username = reader["Username"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            Surname = reader["Surname"].ToString(),
                            PasswordHash = reader["PasswordHash"].ToString()
                            // Map other properties as needed
                        };
                        return user;
                    }
                }
            }

            return null;
        }

        //======================================================= End of Method ===================================================
    }
}
//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> END OF FILE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>