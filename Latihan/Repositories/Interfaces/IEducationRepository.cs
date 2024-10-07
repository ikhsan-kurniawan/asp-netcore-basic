using Latihan.ViewModels;

namespace Latihan.Repositories.Interfaces
{
    public interface IEducationRepository
    {
        public Dictionary<string, int> GetTotalDegree();
    }
}
