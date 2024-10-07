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
    [Authorize(Roles = "Admin, Employee")]
    public class EmployeesController:ControllerBase
    {
        private EmployeeRepositories _employeeRepositories;

        public EmployeesController(EmployeeRepositories employeeRepositories)
        {
            _employeeRepositories = employeeRepositories;
        }

        [HttpGet("{nik}")]
        public IActionResult GetEmployeeById(string nik)
        {
            var claims = User.Claims.Select(c => new
            {
                c.Type,
                c.Value
            }).ToList();

            return Ok(claims);

            var employee = _employeeRepositories.GetEmployeeById(nik);
            if (employee == null)
            {
                return BadRequest(ApiResponse.CreateResponse(400, $"Data {nik} tidak ditemukan"));
            }

            return Ok(ApiResponse.CreateResponse(200, $"Data {nik} berhasil ditemukan", employee));
        }

        [HttpPut("{nik}")]
        public IActionResult UpdateEmployee(string nik, Employee employee)
        {
            if (nik != employee.NIK)
            {
                return BadRequest(ApiResponse.CreateResponse(400, "ID tidak sama"));
            }

            var updatedEmployee = _employeeRepositories.UpdateEmployee(employee);

            if (updatedEmployee == EmployeeRepositories.NOTFOUND)
            {
                return NotFound(ApiResponse.CreateResponse(404, $"Data {nik} tidak ditemukan"));
            }

            return Ok(ApiResponse.CreateResponse(200, $"Data {nik} berhasil diupdate", employee));
        }
    }
}
