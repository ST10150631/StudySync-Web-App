using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace PROG6212_POE_PART_3_ST10150631.Models
{
    public class SemesterModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SemesterModel()
        {
            this.Modules = new HashSet<Module>();
        }
        [Key]
        public int? SemesterID { get; set; }
        public string SemesterName { get; set; }
        public double Weeks { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        [ForeignKey("Username")]
        public string Username { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Module> Modules { get; set; }
        public virtual UserModel User { get; set; }
    }
}
