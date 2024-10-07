using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Latihan.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Employee
    {
        [Key]
        [Required(ErrorMessage ="NIK harus diisi")]
        public string NIK { get; set; }

        [Required(ErrorMessage = "FirstName harus diisi")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName harus diisi")]
        public string LastName { get; set; }

        public string? Phone { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email harus diisi")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [JsonIgnore]
        public virtual Account? Account { get; set; }

    }
}
