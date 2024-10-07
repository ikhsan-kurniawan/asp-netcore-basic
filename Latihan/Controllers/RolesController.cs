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
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private RoleRepositories _roleRepositories;
        
        public RolesController (RoleRepositories roleRepositories)
        {
            _roleRepositories = roleRepositories;
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleRepositories.GetAllRoles();

            if (roles.Count() == 0)
            {
                return NotFound(ApiResponse.CreateResponse(404, "Data tidak ditemukan"));
            }
            return Ok(ApiResponse.CreateResponse(200, $"{roles.Count()} data ditemukan", roles));
        }

        [HttpGet("{roleId}")]
        public IActionResult GetRoleById(string roleId)
        {
            if (!roleId.StartsWith('R') || roleId.Length != 3)
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Format ID salah"));
            }
            var role = _roleRepositories.GetRoleById(roleId);
            if (role == null)
            {
                return BadRequest(ApiResponse.CreateResponse(400, $"Data {roleId} tidak ditemukan"));
            }

            return Ok(ApiResponse.CreateResponse(200, $"Data {roleId} berhasil ditemukan", role));
        }


        [HttpPost]
        public IActionResult AddRole(Role role)
        {
            if (String.IsNullOrWhiteSpace(role.RoleName))
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Nama harus diisi"));
            }
            var addRole = _roleRepositories.AddRoles(role);

            if (addRole <= 0)
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Data gagal ditambahkan"));
            }

            return Ok(ApiResponse.CreateResponse(201, "Data berhasil ditambahkan", role));
        }

        [HttpPut("{roleId}")]
        public IActionResult UpdateRole(string roleId, Role role)
        {
            if (String.IsNullOrWhiteSpace(role.RoleName))
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Nama harus diisi"));
            }
            else if (String.IsNullOrWhiteSpace(role.RoleId))
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Id harus diisi"));
            }

            if (roleId != role.RoleId)
            {
                return BadRequest(ApiResponse.CreateResponse(400, "ID tidak sama"));
            }
            else if (!roleId.StartsWith('R') || roleId.Length != 3)
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Format ID salah"));
            }

            var updatedRole = _roleRepositories.UpdateRole(role);

            if (updatedRole == RoleRepositories.NOTFOUND)
            {
                return NotFound(ApiResponse.CreateResponse(404, $"Data {roleId} tidak ditemukan"));
            }

            return Ok(ApiResponse.CreateResponse(200, $"Data {roleId} berhasil diupdate", role));
        }

        [HttpDelete("{roleId}")]
        public IActionResult DeleteUniversity(string roleId)
        {
            if (!roleId.StartsWith('R') || roleId.Length != 3)
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Format ID salah"));
            }

            try
            {
                int delete = _roleRepositories.DeleteRoleById(roleId);

                if (delete == RoleRepositories.NOTFOUND)
                {
                    return BadRequest(ApiResponse.CreateResponse(400, $"Data {roleId} tidak ditemukan"));
                }

                return Ok(ApiResponse.CreateResponse(200, $"Data {roleId} berhasil dihapus"));
            }
            catch (Exception ex) {
                return StatusCode(500, ApiResponse.CreateResponse(500, "Terjadi kesalahan saat menghapus roles."));
            }
        }
    }
}
