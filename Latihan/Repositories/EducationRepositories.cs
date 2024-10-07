using Latihan.Context;
using Latihan.Repositories.Interfaces;
using Latihan.ViewModels;

namespace Latihan.Repositories
{
    public class EducationRepositories : IEducationRepository
    {
        private readonly MyContext _myContext;

        public EducationRepositories(MyContext myContext)
        {
            _myContext = myContext;
        }
        public Dictionary<string, int> GetTotalDegree()
        {
            var result = _myContext.Educations
                .GroupBy(e => e.Degree)
                .Select(group => new
                {
                    Degree = group.Key.ToString(),
                    Count = group.Count()
                })
                .ToDictionary(g => g.Degree, g => g.Count);

            return result;
        }
    }
}
