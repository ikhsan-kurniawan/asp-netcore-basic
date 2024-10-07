using Latihan.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Latihan.ViewModels
{
    public class EditPasswordVM
    {
        public string? NIK { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? NewPasswordConfirmation { get; set; }
    }
}
