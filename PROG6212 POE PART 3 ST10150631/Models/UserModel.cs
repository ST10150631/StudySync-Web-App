using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using StudySyncClassLibrary.Classes;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PROG6212_POE_PART_3_ST10150631.Models
{
    public class UserModel
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserModel()
        {
            this.Modules = new HashSet<Module>();
            this.Notes = new HashSet<NoteModel>();
            this.Semesters = new HashSet<SemesterModel>();
        }
        [Key] 
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? PasswordHash { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Module> Modules { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NoteModel> Notes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SemesterModel> Semesters { get; set; }
    }
}
//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> END OF FILE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>