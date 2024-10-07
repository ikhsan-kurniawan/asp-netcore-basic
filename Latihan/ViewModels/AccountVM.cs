using Latihan.Models;

namespace Latihan.ViewModels
{
    public class AccountVM
    {
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? UniversityName { get; set; }
        public string? UniversityId { get; set; }
        public float GPA { get; set; }
        public string Degree {  get; set; }
        public Degree? enumDegree {  get; set; }

        public List<string>? Roles { get; set; }

    }
}
