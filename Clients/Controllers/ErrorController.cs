using Microsoft.AspNetCore.Mvc;

namespace Clients.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }        
        public IActionResult Unauthorized()
        {
            return View();
        }        
        public IActionResult Unauthenticated()
        {
            return View();
        }
    }
}
