using Microsoft.Data.SqlClient;
using System.Reflection;
using PROG6212_POE_PART_3_ST10150631.Models;
using System.Data;

namespace PROG6212_POE_PART_3_ST10150631.ViewModels
{
    public class ModuleViewModel
    {
        /// <summary>
        /// Connection String for database
        /// </summary>
        private string connectionString = "Server=tcp:dbs-vc-cldv6212-st10150631.database.windows.net,1433;Initial Catalog=PROG6212_POE_DB;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";";

        /// <summary>
        /// default constructor
        /// </summary>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public ModuleViewModel()
        {
            
        }
        //======================================================= End of Method ===================================================



        //Prog6212test
        //Programming 2A test
        //15
        //
        /// <summary>
        /// Adds a new Module to the Database
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <param name="WeeklyClassHrs"></param>
        /// <param name="WeeklySelfHrs"></param>
        /// <param name="Credits"></param>
        /// <param name="SemesterID"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public async Task AddNewModule(ModuleModel module)
        {
            module.CompletedSelfHrs = 0;
            // Query for inserting data into the database
            string Query = "INSERT INTO dbo.[Module] (ModuleName, ModuleCode, WeeklyClassHrs, WeeklySelfHrs,CompletedSelfHrs,WeekWhenAdded, Credits, SemesterName,SemesterID, Username) " +
                           "VALUES (@ModuleName, @ModuleCode, @WeeklyClassHrs, @WeeklySelfHrs,@CompletedSelfHrs,@WeekWhenAdded, @Credits, @SemesterName,@SemesterID ,@Username);";
            SemesterViewModel sVm = new SemesterViewModel();
            SemesterModel foundSemester = await sVm.SearchSemByName(module.SemesterName);
            double WeeklySelfHrs = CalculateSelfStudyHrs(module.WeeklyClassHrs, module.Credits, foundSemester.Weeks);
            try
            {


                using (SqlConnection SQLconnect = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(Query, SQLconnect);
                    command.Parameters.AddWithValue("@ModuleName", module.ModuleName);
                    command.Parameters.AddWithValue("@ModuleCode", module.ModuleCode);
                    command.Parameters.AddWithValue("@WeeklyClassHrs", module.WeeklyClassHrs);
                    command.Parameters.AddWithValue("@WeeklySelfHrs", WeeklySelfHrs);
                    command.Parameters.AddWithValue("@CompletedSelfHrs", 0);
                    command.Parameters.AddWithValue("@WeekWhenAdded", 0);
                    command.Parameters.AddWithValue("@Credits", module.Credits);
                    command.Parameters.AddWithValue("@SemesterName", module.SemesterName);
                    command.Parameters.AddWithValue("@SemesterID", foundSemester.SemesterID);
                    command.Parameters.AddWithValue("@Username", MainViewModel.SignedInUser);

                    // Opens the SQL connection
                    SQLconnect.Open();

                    // Execute the connection
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch
            {
            }

        }
        //======================================================= End of Method ===================================================

        /// <summary>
        ///  Resets the Hrs studied to 0
        /// </summary>
        /// <param name="username"></param>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public async Task ResetHrsStudied(string username)
        {
            string query = $"UPDATE dbo.[Module] SET CompletedSelfHrs = 0 WHERE Username = '{username}' ;";
            using (SqlConnection sqlConnect = new SqlConnection(connectionString))
            {
                sqlConnect.Open();

                SqlCommand cmd = new SqlCommand(query, sqlConnect);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        //======================================================= End of Method ===================================================

        /// <summary>
        /// Calculates the progressbar value
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public double CalculatePercentage(double complete, double total)
        {
            if (total <= 0)
            {
                // If the divisor is zero, return 100%
                return 100.0;
            }
            else if ( total < complete)
            {
                // If the dividend is less than or equal to zero or the divisor is less than the dividend, return 0%
                return 100.0;
            }
            else if (complete <= 0)
            {
                return 0;
            }
            else
            {
                // Calculate the percentage
                return Math.Round((complete / total) * 100.0);
            }
        }
        //======================================================= End of Method ===================================================




        /// <summary>
        /// Adds new modules to the database  async
        /// </summary>
        /// <returns></returns>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public List<ModuleModel> GetAllModules()
        {
            string username = MainViewModel.SignedInUser;

            List<ModuleModel> modules = new List<ModuleModel>();

            string query = "SELECT * FROM [dbo].[Module] WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();

                using (SqlDataReader reader =  command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ModuleModel module = new ModuleModel
                        {
                            ModuleID = (int)reader["ModuleID"],
                            ModuleCode = reader["ModuleCode"].ToString(),
                            ModuleName = reader["ModuleName"].ToString(),
                            WeeklyClassHrs = Convert.ToDouble(reader["WeeklyClassHrs"]),
                            WeeklySelfHrs = Convert.ToDouble(reader["WeeklySelfHrs"]),
                            CompletedSelfHrs = Convert.ToDouble(reader["CompletedSelfHrs"]),
                            Credits = Convert.ToDouble(reader["Credits"]),
                            SemesterID = (int)reader["SemesterID"],
                            SemesterName = reader["SemesterName"].ToString(),
                            Username = reader["Username"].ToString()
                        };

                        modules.Add(module);
                    }
                }
            }

            return modules;
        }
        //======================================================= End of Method ===================================================


        /// <summary>
        /// Deletes a semsester from the database
        /// </summary>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public async Task DeleteModuleAsync(string ModuleName)
        {
            try
            {
                string username = MainViewModel.SignedInUser;
                string query = "DELETE FROM dbo.[Module] WHERE Username = @Username AND ModuleName = @ModuleName";

                using (SqlConnection SQLconnect = new SqlConnection(connectionString))
                {
                    SQLconnect.Open();

                    SqlCommand cmnd = new SqlCommand(query, SQLconnect);
                    cmnd.Parameters.AddWithValue("@Username", username);
                    cmnd.Parameters.AddWithValue("@ModuleName", ModuleName);

                    await cmnd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, or throw them further as needed
                Console.WriteLine("Error deleting semester: " + ex.Message);
                throw;
            }
        }
        //======================================================= End of Method ===================================================



        /// <summary>
        /// Calculates the number of weekly self study hours required by a module
        /// </summary>
        /// <param name="ClassHrs"></param>
        /// <param name="credits"></param>
        /// <param name="weeks"></param>
        /// <returns></returns>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public double CalculateSelfStudyHrs(double ClassHrs, double credits, double weeks)
        {
            double SelfStudyHrs = 0;
            SelfStudyHrs = (credits * 10) / weeks - ClassHrs;
            if (SelfStudyHrs < 0)
            {
                SelfStudyHrs = 0;
            }
            SelfStudyHrs = Math.Round(SelfStudyHrs, 2);
            return SelfStudyHrs;

        }
        //======================================================= End of Method ===================================================

        /// <summary>
        /// Adds Self study Hours completed and the week completed
        /// </summary>
        /// <param name="ModName"></param>
        /// <param name="HrsStudied"></param>
        /// <returns></returns>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public async Task AddHrsStudied(string ModName, double HrsStudied)
        {
            string username = MainViewModel.SignedInUser;

            string query = "UPDATE dbo.[Module] SET CompletedSelfHrs = @HrsStudied WHERE Username = @Username AND ModuleName = @ModName";

            using (SqlConnection sqlConnect = new SqlConnection(connectionString))
            {
                sqlConnect.Open();

                using (SqlCommand cmd = new SqlCommand(query, sqlConnect))
                {
                    cmd.Parameters.Add(new SqlParameter("@HrsStudied", SqlDbType.Float)).Value = HrsStudied;
                    cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar)).Value = username;
                    cmd.Parameters.Add(new SqlParameter("@ModName", SqlDbType.NVarChar)).Value = ModName;

                   await  cmd.ExecuteNonQueryAsync();
                }
            }
        }
        //======================================================= End of Method ===================================================
    }
}
//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> END OF FILE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>