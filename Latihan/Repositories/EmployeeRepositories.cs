using Latihan.Context;
using Latihan.Models;
using Latihan.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Latihan.Repositories
{
    public class EmployeeRepositories : IEmployeeRepository
    {
        public const int NOTFOUND = -3;

        private readonly MyContext _myContext;
        public EmployeeRepositories(MyContext mycontext) {
            _myContext = mycontext;
        }

        public Employee GetEmployeeById(string id)
        {
            return _myContext.Employees.FirstOrDefault(u => u.NIK == id);
        }

        public int UpdateEmployee(Employee employee)
        {
            var check = GetEmployeeById(employee.NIK);
            if (check == null)
            {
                return NOTFOUND;
            }

            _myContext.Entry(check).State = EntityState.Detached;
            _myContext.Entry(employee).State = EntityState.Modified;

            return _myContext.SaveChanges();        }
    }
}
