using Latihan.Context;
using Latihan.Models;
using Latihan.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Latihan.Repositories
{
    public class RoleRepositories : IRoleRepository
    {
        public const int NOTFOUND = -3;

        private readonly MyContext _myContext;
        public RoleRepositories(MyContext myContext) { 
            _myContext = myContext;
        }
        public int AddRoles(Role role)
        {
            var totalRole = _myContext.Roles.Count();
            var idRole = $"R{++totalRole:D2}";
            role.RoleId = idRole;

            _myContext.Roles.Add(role);
            return _myContext.SaveChanges();
        }

        public int DeleteRoleById(string id)
        {
            var roleToDelete = GetRoleById(id);
            if (roleToDelete == null)
            {
                return NOTFOUND;
            }
            _myContext.Roles.Remove(roleToDelete);
            return _myContext.SaveChanges();
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _myContext.Roles.ToList();
        }

        public Role GetRoleById(string id)
        {
            return _myContext.Roles.FirstOrDefault(u => u.RoleId == id);
        }

        public int UpdateRole(Role role)
        {
            var check = GetRoleById(role.RoleId);
            if (check == null)
            {
                return NOTFOUND;
            }

            _myContext.Entry(check).State = EntityState.Detached;
            _myContext.Entry(role).State = EntityState.Modified;

            return _myContext.SaveChanges();
        }
    }
}
