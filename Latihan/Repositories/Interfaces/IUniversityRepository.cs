using Latihan.Models;

namespace Latihan.Repositories.Interfaces
{
    public interface IUniversityRepository
    {
        IEnumerable<University> GetAllUniversity();
        University GetUniversityById(string id);
        int AddUniversity(University university);
        int UpdateUniversity(University university);
        int DeleteUniversityById(string id);
    }
}
