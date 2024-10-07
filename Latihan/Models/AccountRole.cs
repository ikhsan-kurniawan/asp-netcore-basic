using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Latihan.Models
{
    public class AccountRole
    {
        [Key]
        public int AccountRoleId { get; set; }

        public virtual Account? Account { get; set; }
        [ForeignKey("Account")]
        public string? NIK { get; set; }

        public virtual Role? Role { get; set; }
        [ForeignKey("Role")]
        public string? RoleId { get; set; }

    }
}
