using Latihan.Context;
using Latihan.Models;
using Latihan.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Latihan.Repositories
{
    public class UniversityRepositories : IUniversityRepository
    {
        public const int NOTFOUND = -3;

        private readonly MyContext _myContext;
        public UniversityRepositories(MyContext myContext)
        {
            _myContext = myContext;
        }
        public int AddUniversity(University university)
        {
            var totalUniversity = _myContext.Universities.Count();
            var idUniversity = $"U{++totalUniversity:D3}";
            university.Id = idUniversity;

            _myContext.Universities.Add(university);
            return _myContext.SaveChanges();
        }

        public int DeleteUniversityById(string id)
        {
            var universityToDelete = GetUniversityById(id);
            if (universityToDelete == null)
            {
                return NOTFOUND;
            }
            _myContext.Universities.Remove(universityToDelete);
            return _myContext.SaveChanges();
            //throw new NotImplementedException();
        }

        public IEnumerable<University> GetAllUniversity()
        {
            return _myContext.Universities.ToList();
        }

        public University GetUniversityById(string id)
        {
            return _myContext.Universities.FirstOrDefault(u => u.Id == id);
            //throw new NotImplementedException();
        }

        public int UpdateUniversity(University university)
        {
            var check = GetUniversityById(university.Id);
            if (check == null)
            {
                return NOTFOUND;
            }

            _myContext.Entry(check).State = EntityState.Detached;
            _myContext.Entry(university).State = EntityState.Modified;

            return _myContext.SaveChanges();
            
            //throw new NotImplementedException();
        }
    }
}
