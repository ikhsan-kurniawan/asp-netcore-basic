using Latihan.Helpers;
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

    public class AccountRolesController : ControllerBase
    {
        private AccountRolesRepositories _accountRolesRepositories;

        public AccountRolesController(AccountRolesRepositories accountRolesRepositories)
        {
            _accountRolesRepositories = accountRolesRepositories;
        }

        [HttpGet]
        public IActionResult GetAccountRoles()
        {
            var accountRoles = _accountRolesRepositories.GetAllAccountRoles();

            if (accountRoles.Count() == 0)
            {
                return NotFound(ApiResponse.CreateResponse(404, "Data tidak ditemukan"));
            }
            return Ok(ApiResponse.CreateResponse(200, $"{accountRoles.Count()} data ditemukan", accountRoles));
        }

    }
}
