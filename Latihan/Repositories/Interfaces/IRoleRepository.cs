using Latihan.Models;

namespace Latihan.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetAllRoles();
        Role GetRoleById(string id);
        int AddRoles(Role role);
        int UpdateRole(Role role);
        int DeleteRoleById(string id);
    }
}
