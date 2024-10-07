using Latihan.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Latihan.ViewModels
{
    public class RegisterVM
    {
        public string? NIK { get; set; }

        [Required(ErrorMessage = "FirstName harus diisi")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName harus diisi")]
        public string LastName { get; set; }

        public string? Phone { get; set; }

        public DateTime? BirthDate { get; set; }


        [EmailAddress]
        [Required(ErrorMessage = "Email harus diisi")]
        public string Email { get; set; }

        public string? Password { get; set; }

        [Required(ErrorMessage = "University_Id harus diisi")]
        public string University_Id { get; set; }

        [EnumDataType(typeof(Degree))]
        [Required(ErrorMessage = "Degree harus diisi")]
        public Degree Degree { get; set; }

        [Required(ErrorMessage = "GPA harus diisi")]
        //[Range(0, 4.00)]
        public float GPA { get; set; }

        //public string? Role { get; set; }


    }

    public class LoginVM
    {
        public string? EmailOrNik { get; set; }

        public string? Password { get; set; }
    }
}
