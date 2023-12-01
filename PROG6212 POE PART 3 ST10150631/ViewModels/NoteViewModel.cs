using Microsoft.Data.SqlClient;
using PROG6212_POE_PART_3_ST10150631.Models;
using System.Reflection.PortableExecutable;

namespace PROG6212_POE_PART_3_ST10150631.ViewModels
{
    public class NoteViewModel
    {
        private string connectionString = "Server=tcp:dbs-vc-cldv6212-st10150631.database.windows.net,1433;Initial Catalog=PROG6212_POE_DB;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";";


        /// <summary>
        /// defaut constructor 
        /// </summary>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public NoteViewModel()
        {
            
        }
        //======================================================= End of Method ===================================================

        /// <summary>
        /// Checks if a notes should be displayed
        /// </summary>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public NoteModel CheckNotes()
        {
            try
            {
                // Query for inserting data into the database
                string Query = "SELECT * FROM dbo.[Notes] WHERE Username = @Username AND NoteDate < @Date";

                using (SqlConnection SQLconnect = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(Query, SQLconnect);
                    command.Parameters.AddWithValue("@Username", MainViewModel.SignedInUser);
                    command.Parameters.AddWithValue("@Date", DateTime.Now);

                    // Opens the SQL connection
                    SQLconnect.Open();

                    // Execute the query and retrieve the result
                    SqlDataReader reader = command.ExecuteReader();

                    if ( reader.Read())
                    {
                        NoteModel note = new NoteModel
                        {
                            NoteID = (int)reader["NoteID"],
                            NoteName = reader["NoteName"].ToString(),
                            NoteContent = reader["NoteContent"].ToString(),
                            NoteDate = Convert.ToDateTime(reader["NoteDate"]),
                            Username = reader["Username"].ToString()
                        };
                        return note;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return null;
        }
        //======================================================= End of Method ===================================================

        /// <summary>
        /// Deletes a note once shown 
        /// </summary>
        /// <returns></returns>
        public void RemoveNote(string noteName)
        {
                string username = MainViewModel.SignedInUser;
                string query = "DELETE FROM dbo.[Notes] WHERE Username = @Username AND NoteName = @NoteName";

                using (SqlConnection SQLconnect = new SqlConnection(connectionString))
                {
                    SQLconnect.Open();

                    SqlCommand cmnd = new SqlCommand(query, SQLconnect);
                    cmnd.Parameters.AddWithValue("@Username", username);
                    cmnd.Parameters.AddWithValue("@NoteName", noteName);

                     cmnd.ExecuteNonQuery();
                }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// ----------------------------------------------------- Start of Method ------------------------------------------------
        public async Task AddNote(NoteModel note)
        {
            try
            {
                // Query for inserting data into the database
                string Query = "INSERT INTO dbo.[Notes] (NoteName, NoteContent, NoteDate, Username) VALUES (@NoteName, @Description,@Date, @Username)";

                using (SqlConnection SQLconnect = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(Query, SQLconnect);
                    command.Parameters.AddWithValue("@NoteName", note.NoteName);
                    command.Parameters.AddWithValue("@Description", note.NoteContent);
                    command.Parameters.AddWithValue("@Username", note.Username);
                    command.Parameters.AddWithValue("@Date", note.NoteDate);

                    // Opens the SQL connection
                    SQLconnect.Open();

                    // Execute the connection
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
        }


    }
}
//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> END OF FILE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>