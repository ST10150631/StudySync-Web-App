using Microsoft.Data.SqlClient;
using PROG6212_POE_PART_3_ST10150631.Models;

namespace PROG6212_POE_PART_3_ST10150631.ViewModels
{
    public class SemesterViewModel
    {
        /// <summary>
        /// Connection String for database
        /// </summary>
        private string connectionString = "Server=tcp:dbs-vc-cldv6212-st10150631.database.windows.net,1433;Initial Catalog=PROG6212_POE_DB;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";";

        /// <summary>
        /// deafult constructor
        /// </summary>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public SemesterViewModel()
        {
            
        }
        //======================================================= End of Method ===================================================



        /// <summary>
        /// Asynchronously Adds a Semester to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>\
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public async Task<SemesterModel> AddSemesterToDBAsync(SemesterModel model)
        {
            //Query for inserting data into the database 
            string Query = "INSERT INTO dbo.[Semester] (SemesterName, Weeks, StartDate, EndDate, Username ) VALUES (@SemesterName,@Weeks,@StartDate,@EndDate,@Username);";

            var newSem = model;
            DateTime EndDate = CalculateEndDate(model.StartDate, model.Weeks);

            //Opens and closses the SQL connection
            using (SqlConnection SQLconnect = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(Query, SQLconnect);
                command.Parameters.AddWithValue("@SemesterName", model.SemesterName);
                command.Parameters.AddWithValue("@Weeks", model.Weeks);
                command.Parameters.AddWithValue("@StartDate", model.StartDate);
                command.Parameters.AddWithValue("@EndDate", EndDate);
                command.Parameters.AddWithValue("@Username", MainViewModel.SignedInUser);

                SQLconnect.Open();
                //Execute the connection
                await command.ExecuteNonQueryAsync();


            }
            return newSem;
        }
        //======================================================= End of Method ===================================================

        /// <summary>
        /// Searches for a semester in the database using SemesterName and returns semester 
        /// </summary>
        /// <param name="SemesterName"></param>
        /// <returns></returns>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public async Task<SemesterModel> SearchSemByName(string SemName)
        {
            string username = MainViewModel.SignedInUser;
            string query = $"SELECT * FROM dbo.[Semester] WHERE SemesterName = '{SemName}' AND Username = '{username}';";

            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                SqlCommand command = new SqlCommand(query, connect);

                // Use parameters to avoid SQL injection
                command.Parameters.AddWithValue("@SemName", SemName);
                command.Parameters.AddWithValue("@username", username);

                // Execute the query and retrieve the result
                SqlDataReader reader = command.ExecuteReader();

                if (await reader.ReadAsync())
                {
                    SemesterModel semester = new SemesterModel
                    {
                        SemesterID = reader["SemesterID"] as int?,
                        SemesterName = reader["SemesterName"].ToString(),
                        Weeks = Convert.ToDouble(reader["Weeks"]),
                        StartDate = Convert.ToDateTime(reader["StartDate"]),
                        EndDate = Convert.ToDateTime(reader["EndDate"]),
                        Username = reader["Username"].ToString()
                    };
                    return semester;
                }
            }
            return null;
        }
        //======================================================= End of Method ===================================================

        /// <summary>
        /// Gets all the semesters from the databas based on the username
        /// </summary>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public List<SemesterModel> GetAllSemesters(string username)
        {
            List<SemesterModel> semesters = new List<SemesterModel>();

            string query = "SELECT * FROM [dbo].[Semester] WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SemesterModel semester = new SemesterModel
                        {
                            SemesterID = reader["SemesterID"] as int?,
                            SemesterName = reader["SemesterName"].ToString(),
                            Weeks = Convert.ToDouble(reader["Weeks"]),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            EndDate = Convert.ToDateTime(reader["EndDate"]),
                            Username = reader["Username"].ToString()
                        };

                        semesters.Add(semester);
                    }
                }
            }

            return semesters;
        }
        //======================================================= End of Method ===================================================

        /// <summary>
        /// Deletes a semsester from the database
        /// </summary>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public async Task DeleteSemesterAsync(string SemName)
        {
            try
            {
                string username = MainViewModel.SignedInUser;
                string query = "DELETE FROM dbo.[Semester] WHERE Username = @Username AND SemesterName = @SemesterName";

                using (SqlConnection SQLconnect = new SqlConnection(connectionString))
                {
                    SQLconnect.Open();

                    SqlCommand cmnd = new SqlCommand(query, SQLconnect);
                    cmnd.Parameters.AddWithValue("@Username", username);
                    cmnd.Parameters.AddWithValue("@SemesterName", SemName);

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
        /// Calculates the end date of a semester
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="Weeks"></param>
        /// <returns></returns>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        private DateTime CalculateEndDate(DateTime StartDate, double Weeks)
        {
            DateTime EndDate;
            double SemDays = Weeks * 7;
            StartDate = StartDate.Date;
            EndDate = StartDate.Date;
            EndDate = EndDate.AddDays(SemDays).Date;
            return EndDate;
        }
        //======================================================= End of Method ===================================================

    }
}
//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> END OF FILE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>