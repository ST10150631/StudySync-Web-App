using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG6212_POE_PART_3_ST10150631.Models
{
    public class ModuleModel
    {
        [Key]
        public int ModuleID { get; set; }
        public string? ModuleCode { get; set; }
        public string? ModuleName { get; set; }
        public double WeeklyClassHrs { get; set; }
        public double WeeklySelfHrs { get; set; }
        public double ProgressBarValue { get; set; }
        public double CompletedSelfHrs { get; set; }
        public double Credits { get; set; }
        public List<SemesterModel> SemesterList = new List<SemesterModel>();
        [ForeignKey("SemesterID")]
        public int SemesterID { get; set; }
        public string? SemesterName { get; set; }
        [ForeignKey("Username")]
        public string? Username { get; set; }

        public virtual SemesterModel Semester { get; set; }
        public virtual UserModel User { get; set; }
    }
}
