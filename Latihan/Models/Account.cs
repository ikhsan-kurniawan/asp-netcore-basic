using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Latihan.Models
{
    public class Account
    {
        public virtual Employee? Employee { get; set; }
        [Key]
        [ForeignKey("Employee")]
        [Required(ErrorMessage = "NIK harus diisi")]
        public string NIK { get; set; }

        [Required(ErrorMessage = "Password harus diisi")]
        public string Password { get; set; }

        public virtual Profilling? Profilling { get; set; }
        public virtual ICollection<AccountRole>? AccountRole { get; set; }

    }
}
