using Clients.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Clients.Controllers
{
    [RoleAuthorize("Admin")]
    public class RolesController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Data Roles";
            ViewBag.ActivePage = "Roles";
            return View();
        }
    }
}
