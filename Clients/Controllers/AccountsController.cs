using Clients.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Clients.Controllers
{
    public class AccountsController : Controller
    {
        [RoleAuthorize("Admin")]
        public IActionResult Index()
        {
            ViewBag.Title = "Data Accounts";
            ViewBag.ActivePage = "Accounts";
            ViewBag.Username = User.Identity.Name;
            return View();
        }
    }
}
