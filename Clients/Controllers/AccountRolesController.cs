using Clients.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Clients.Controllers
{
    public class AccountRolesController : Controller
    {
        [RoleAuthorize("Admin")]
        public IActionResult Index()
        {
            ViewBag.Title = "Data Account Roles";
            ViewBag.ActivePage = "AccountRoles";
            return View();
        }
    }
}
