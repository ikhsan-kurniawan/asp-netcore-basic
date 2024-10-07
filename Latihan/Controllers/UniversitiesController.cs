using Latihan.Helpers;
using Latihan.Models;
using Latihan.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Latihan.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    [Authorize(Roles ="Admin, Employee")]
    public class UniversitiesController : ControllerBase
    {
        private UniversityRepositories _universityRepositories;

        public UniversitiesController(UniversityRepositories universityRepositories)
        {
            _universityRepositories = universityRepositories;
        }

        [HttpGet]
        public IActionResult GetAllUniversities()
        {
            var universities = _universityRepositories.GetAllUniversity();

            if (universities.Count() == 0)
            {
                return NotFound(ApiResponse.CreateResponse(404, "Data tidak ditemukan"));
            }
            return Ok(ApiResponse.CreateResponse(200, $"{universities.Count()} data ditemukan", universities));
        }

        [HttpGet("{univId}")]
        public IActionResult GetUniversityById(string univId)
        {
            if (!univId.StartsWith('U') || univId.Length != 4)
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Format ID salah"));
            }
            var university = _universityRepositories.GetUniversityById(univId);
            if (university == null)
            {
                return BadRequest(ApiResponse.CreateResponse(400, $"Data {univId} tidak ditemukan"));
            }

            return Ok(ApiResponse.CreateResponse(200, $"Data {univId} berhasil ditemukan", university));
        }

        [HttpPost]
        public IActionResult AddUniversity(University university)
        {
            if (String.IsNullOrWhiteSpace(university.Name))
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Nama harus diisi"));
            }
            var addUniversity = _universityRepositories.AddUniversity(university);

            if (addUniversity <= 0)
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Data gagal ditambahkan"));
            }

            return Ok(ApiResponse.CreateResponse(201, "Data berhasil ditambahkan", university));
        }

        [HttpPut("{univId}")]
        public IActionResult UpdateUniversity(string univId, University university)
        {
            if (String.IsNullOrWhiteSpace(university.Name))
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Nama harus diisi"));
            }
            else if (String.IsNullOrWhiteSpace(university.Id))
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Id harus diisi"));
            }

            if (univId != university.Id)
            {
                return BadRequest(ApiResponse.CreateResponse(400, "ID tidak sama"));
            } else if (!univId.StartsWith('U') || univId.Length != 4 )
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Format ID salah"));
            }

            var updatedUniversity = _universityRepositories.UpdateUniversity(university);

            if (updatedUniversity == UniversityRepositories.NOTFOUND)
            {
                return NotFound(ApiResponse.CreateResponse(404, $"Data {univId} tidak ditemukan"));
            }

            return Ok(ApiResponse.CreateResponse(200, $"Data {univId} berhasil diupdate", university));
        }

        [HttpDelete("{univId}")]
        public IActionResult DeleteUniversity(string univId)
        {
            if (!univId.StartsWith('U') || univId.Length != 4)
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Format ID salah"));
            }

            int delete = _universityRepositories.DeleteUniversityById(univId);

            if (delete == UniversityRepositories.NOTFOUND)
            {
                return BadRequest(ApiResponse.CreateResponse(400, $"Data {univId} tidak ditemukan"));
            }

            return Ok(ApiResponse.CreateResponse(200, $"Data {univId} berhasil dihapus"));
        }
    }
}
