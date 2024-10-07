using Microsoft.AspNetCore.Mvc;

namespace Clients.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            var rolesSession = HttpContext.Session.GetString("roles");
            if (!string.IsNullOrEmpty(rolesSession))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }        
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
