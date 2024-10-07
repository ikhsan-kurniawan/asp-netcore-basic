using Latihan.Models;

namespace Latihan.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Employee GetEmployeeById(string id);

        int UpdateEmployee(Employee employee);
    }
}
