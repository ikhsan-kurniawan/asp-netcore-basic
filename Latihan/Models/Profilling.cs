using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Latihan.Models
{
    public class Profilling
    {
        public virtual Account? Account { get; set; }
        [Key]
        [ForeignKey("Account")]
        [Required(ErrorMessage = "NIK harus diisi")]
        public string NIK { get; set; }

        public virtual Education? Education { get; set; }
        [ForeignKey("Education")]
        [Required(ErrorMessage = "Education_Id harus diisi")]
        public string Education_Id { get; set; }
    }
}
