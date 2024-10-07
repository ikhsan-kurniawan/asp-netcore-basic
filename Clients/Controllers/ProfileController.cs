using Clients.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Clients.Controllers
{
    public class ProfileController : Controller
    {
        [RoleAuthorize("Admin", "Employee")]
        public IActionResult Index()
        {
            var rolesSession = HttpContext.Session.GetString("roles");
            ViewBag.Roles = rolesSession;

            ViewBag.Title = "Profile";
            ViewBag.ActivePage = "Profile";
            return View();
        }
    }
}
