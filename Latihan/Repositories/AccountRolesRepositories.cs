using Latihan.Context;
using Latihan.Models;
using Latihan.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Latihan.Repositories
{
    public class AccountRolesRepositories
    {

        private readonly MyContext _myContext;
        public AccountRolesRepositories(MyContext myContext)
        {
            _myContext = myContext;
        }

        public IEnumerable<AccountRoleVM> GetAllAccountRoles()
        {
            var accountRoles = _myContext.AccountRoles
                .Include(ar => ar.Account)
                .Include(ar => ar.Role)
                .Select(ar => new AccountRoleVM
                {
                    AccountRoleId = ar.AccountRoleId,
                    AccountName = ar.Account != null ? ar.Account.Employee.FirstName + " " + ar.Account.Employee.LastName : null,
                    RoleName = ar.Role != null ? ar.Role.RoleName : null
                })
                .ToList();

            return accountRoles;
        }
    }
}
