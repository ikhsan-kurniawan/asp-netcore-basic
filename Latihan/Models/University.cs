using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Latihan.Models
{
    public class University
    {
        [Key]
        public string? Id { get; set; }

        //[Required(ErrorMessage = "Name harus diisi")]
        public string? Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Education>? Education { get; set; }
    }
}
