using Microsoft.AspNetCore.Mvc;

namespace Clients.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessionController : Controller
    {
        [HttpPost("SetSession")]
        public IActionResult SetSession(SessionVM session)
        {
            //return Ok(new { session});
            if (session == null || string.IsNullOrEmpty(session.nik) || string.IsNullOrEmpty(session.nama))
            {
                return BadRequest(new { message = "shit bro Invalid session data." });
            }

            HttpContext.Session.SetString("nik", session.nik);
            HttpContext.Session.SetString("nama", session.nama);
            HttpContext.Session.SetString("roles", string.Join(",", session.roles ?? new List<string>()));

            return Ok($"Sessions saved");
        }

        [HttpGet]
        public IActionResult GetSession()
        {
            var sessionData = new SessionVM
            {
                nik = HttpContext.Session.GetString("nik"),
                nama = HttpContext.Session.GetString("nama"),
                roles = HttpContext.Session.GetString("roles")?.Split(',').ToList()
            };

            return Ok(sessionData);
        }

        [HttpPost("RemoveSession")]
        public IActionResult RemoveSession()
        {
            HttpContext.Session.Remove("nik");
            HttpContext.Session.Remove("nama");
            HttpContext.Session.Remove("roles");

            return Ok("Sessions removed");
        }
    }

    public class SessionVM
    {
        public string? nik { get; set; }
        public string? nama { get; set; }
        public List<string>? roles { get; set; }
    }
}
