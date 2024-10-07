using Latihan.Models;
using Latihan.ViewModels;

namespace Latihan.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        IEnumerable<AccountVM> GetAllAccounts();
        AccountVM GetAccountById(string id);
        int UpdateAccount(RegisterVM account);
        int Register(RegisterVM registerVM);
        Account Login(LoginVM loginVM);
        int UpdatePassword(EditPasswordVM editPassword);

    }
}
