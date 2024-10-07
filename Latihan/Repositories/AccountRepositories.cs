using Latihan.Context;
using Latihan.Helpers;
using Latihan.Models;
using Latihan.Repositories.Interfaces;
using Latihan.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Latihan.Repositories
{
    public class AccountRepositories : IAccountRepository
    {
        public const int EMAILDUPLICATE = -1;
        public const int PHONEDUPLICATE = -2;
        public const int NOTFOUND = -3;
        public const int PASSWORDNOTMATCH = -4;


        private readonly MyContext _myContext;

        public AccountRepositories(MyContext myContext)
        {
            _myContext = myContext;
        }

        public IEnumerable<AccountVM> GetAllAccounts()
        {
            var accounts = _myContext.Accounts
                .Include(a => a.Employee)
                .Include(a => a.Profilling).ThenInclude(p => p.Education).ThenInclude(e => e.University)
                .Include(a => a.AccountRole).ThenInclude(ar => ar.Role)
                .Select(a => new AccountVM()
                {
                    NIK = a.NIK,
                    FirstName = a.Employee.FirstName,
                    LastName = a.Employee.LastName,
                    Phone = a.Employee.Phone,
                    Email = a.Employee.Email,
                    //BirthDate = $"{a.Employee.BirthDate.ToString().Substring(0, 10)}",
                    BirthDate = a.Employee.BirthDate,
                    //BirthDate = a.Employee.BirthDate.Value.ToString("dd MMMM yyyy"),
                    UniversityName = a.Profilling.Education.University.Name,
                    GPA = a.Profilling.Education.GPA,
                    Degree = a.Profilling.Education.Degree.ToString(),
                    Roles = a.AccountRole.Select(ar => ar.Role.RoleName).ToList() 
                });

            return accounts;
            //throw new NotImplementedException();
        }

        public Account Login(LoginVM loginVM)
        {
            return _myContext.Accounts
                .Include(a => a.Employee)
                .Include(a => a.Profilling).ThenInclude(p => p.Education).ThenInclude(e => e.University)
                .Include(a => a.AccountRole)
                .FirstOrDefault(a => a.NIK == loginVM.EmailOrNik || a.Employee.Email == loginVM.EmailOrNik);
        }

        public int Register(RegisterVM registerVM)
        {
            var isEmailDuplicate = _myContext.Employees.Where(e => e.Email == registerVM.Email).Any();
            if (isEmailDuplicate)
            {
                return EMAILDUPLICATE;
            }

            var isPhoneDuplicate = _myContext.Employees.Where(e => e.Phone == registerVM.Phone).Any();
            if (isPhoneDuplicate)
            {
                return PHONEDUPLICATE;
            }

            var date = DateTime.Now;
            var totalEmployee = _myContext.Employees.Count();
            var NikEmployee = $"{date.Year}{date.Month:D2}{++totalEmployee:D4}";

            Employee employee = new Employee
            {
                NIK = NikEmployee,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Phone = registerVM.Phone,
                Email = registerVM.Email,
                BirthDate = registerVM.BirthDate,
            };
            _myContext.Employees.Add(employee);

            Account account = new Account
            {
                NIK = NikEmployee,
                Password = Hashing.HashPassword(registerVM.Password),
            };
            _myContext.Accounts.Add(account);

            var totalEducation = _myContext.Educations.Count();
            var idEducation = $"E{++totalEducation:D3}";
            Profilling profilling = new Profilling
            {
                NIK = NikEmployee,
                Education_Id = idEducation
            };
            _myContext.Profilling.Add(profilling);

            Education education = new Education
            {
                Id = idEducation,
                Degree = registerVM.Degree,
                GPA = registerVM.GPA,
                University_Id = registerVM.University_Id
            };
            _myContext.Educations.Add(education);

            AccountRole accountRole = new AccountRole
            {
                NIK = NikEmployee,
                RoleId = "R02"
            };
            _myContext.AccountRoles.Add(accountRole);

            return _myContext.SaveChanges();
        }

        public IEnumerable<AccountRoleVM> GetAccountRole(string nik)
        {
            return _myContext.AccountRoles
                .Include(ar => ar.Account)
                .Include(ar => ar.Role)
                .Where(ar => ar.NIK == nik)
                .Select(ar => new AccountRoleVM
                {
                    AccountRoleId = ar.AccountRoleId,
                    AccountName = ar.Account.Employee.FirstName + " " + ar.Account.Employee.LastName,
                    RoleName = ar.Role.RoleName
                })
                .ToList();
        }

        public AccountVM GetAccountById(string id)
        {
            var account = _myContext.Accounts
                .Include(a => a.Employee)
                .Include(a => a.Profilling).ThenInclude(p => p.Education).ThenInclude(e => e.University)
                .Include(a => a.AccountRole).ThenInclude(ar => ar.Role)
                .Where(a => a.NIK == id)
                .Select(a => new AccountVM()
                {
                    NIK = a.NIK,
                    FirstName = a.Employee.FirstName,
                    LastName = a.Employee.LastName,
                    Phone = a.Employee.Phone,
                    Email = a.Employee.Email,
                    BirthDate = a.Employee.BirthDate,
                    //BirthDate = a.Employee.BirthDate.Value.ToString("dd MMMM yyyy"),
                    UniversityName = a.Profilling.Education.University.Name,
                    UniversityId = a.Profilling.Education.University_Id,
                    GPA = a.Profilling.Education.GPA,
                    Degree = a.Profilling.Education.Degree.ToString(),
                    enumDegree = a.Profilling.Education.Degree,
                    Roles = a.AccountRole.Select(ar => ar.Role.RoleName).ToList()
                })
                .FirstOrDefault();

            return account; 
            //throw new NotImplementedException();
        }

        public int UpdateAccount(RegisterVM account)
        {
            var check = GetAccountById(account.NIK);
            if (check == null)
            {
                return NOTFOUND;
            }

            Employee employee = new Employee
            {
                NIK = account.NIK,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Phone = account.Phone,
                Email = account.Email,
                BirthDate = account.BirthDate,
            };
            _myContext.Employees.Update(employee);
            //_myContext.Entry(employee).State = EntityState.Modified;

            Education education = _myContext.Profilling
                .Where(p => p.NIK == account.NIK)
                .Select(p => p.Education)
                .FirstOrDefault();
            education.Degree = account.Degree;
            education.GPA = account.GPA;
            education.University_Id = account.University_Id;
            _myContext.Educations.Update(education);

            return _myContext.SaveChanges();
        }

        public int UpdatePassword(EditPasswordVM editPassword)
        {
            Account account = _myContext.Accounts.FirstOrDefault(a => a.NIK == editPassword.NIK);
            if (account == null)
            {
                return NOTFOUND;
            }
            if (!Hashing.ValidatePassword(editPassword.OldPassword, account.Password))
            {
                return PASSWORDNOTMATCH;
            }

            account.Password = Hashing.HashPassword(editPassword.NewPassword);

            _myContext.Entry(account).State = EntityState.Modified;

            return _myContext.SaveChanges();
        }
    }
}
