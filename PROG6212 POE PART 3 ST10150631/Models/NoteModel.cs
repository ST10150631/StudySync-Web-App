using System.ComponentModel.DataAnnotations;

namespace PROG6212_POE_PART_3_ST10150631.Models
{
    public class NoteModel
    {
        [Key]
        public int NoteID { get; set; }
        public string NoteName { get; set; }
        public string NoteContent { get; set; }
        public System.DateTime NoteDate { get; set; }
        public string Username { get; set; }

        public virtual UserModel User { get; set; }
    }
}
