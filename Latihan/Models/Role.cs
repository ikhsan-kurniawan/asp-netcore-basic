using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Latihan.Models
{
    public class Role
    {
        [Key]
        public string? RoleId {  get; set; }
        public string RoleName { get; set; }
        [JsonIgnore]
        public virtual ICollection<AccountRole>? AccountRole { get; set; }

    }
}
