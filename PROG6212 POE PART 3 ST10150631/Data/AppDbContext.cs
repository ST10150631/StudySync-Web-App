using Microsoft.EntityFrameworkCore;
using PROG6212_POE_PART_3_ST10150631.Models;
namespace PROG6212_POE_PART_3_ST10150631.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure your connection string here
            optionsBuilder.UseSqlServer("Server=tcp:dbs-vc-cldv6212-st10150631.database.windows.net,1433;Initial Catalog=PROG6212_POE_DB;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";");
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<SemesterModel> Semesters { get; set; }
        public DbSet<ModuleModel> Modules { get; set; }
        public DbSet<NoteModel> Notes { get; set; }
    }
}
