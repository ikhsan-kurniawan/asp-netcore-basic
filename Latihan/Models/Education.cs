using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Latihan.Models
{
    public enum Degree
    {
        D3,
        D4,
        S1,
        S2,
        S3
    }
    public class Education
    {
        public virtual ICollection<Profilling>? Profilling { get; set; }

        [Key]
        public string? Id { get; set; }

        [EnumDataType(typeof(Degree))]
        [Required(ErrorMessage = "Degree harus diisi")]
        public Degree Degree { get; set; }

        [Required(ErrorMessage = "GPA harus diisi")]
        [Range(0, 4.00)]
        public float GPA { get; set; }

        public virtual University? University { get; set; }
        [Required(ErrorMessage = "University_Id harus diisi")]
        [ForeignKey("University")]
        public string University_Id { get; set; }

    }
}
