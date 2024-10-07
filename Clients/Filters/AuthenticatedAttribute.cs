using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Clients.Filters
{
    public class AuthenticatedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Check if the user is authenticated using a session variable
            var isAuthenticated = context.HttpContext.Session.GetString("nik") == "true";

            if (!isAuthenticated)
            {
                // Redirect to the unauthenticated page if not authenticated
                context.Result = new RedirectToActionResult("Unauthenticated", "Error", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
