using Clients.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Clients.Controllers
{
    public class DashboardController : Controller
    {
        [RoleAuthorize("Admin", "Employee")]
        public IActionResult Index()
        {
            ViewBag.Title = "Dashboard";
            ViewBag.ActivePage = "Dashboard";

            return View();
        }
    }
}
