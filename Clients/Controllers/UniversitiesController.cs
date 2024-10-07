using Clients.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Clients.Controllers
{
    [RoleAuthorize("Admin")]
    public class UniversitiesController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Data Universities";
            ViewBag.ActivePage = "Universities";
            ViewBag.Menu = "Universities";
            return View();
        }
    }
}
