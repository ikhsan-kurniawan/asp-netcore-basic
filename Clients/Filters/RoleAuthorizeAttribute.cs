using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Clients.Filters
{
    public class RoleAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string[] _allowedRoles;

        public RoleAuthorizeAttribute(params string[] roles)
        {
            _allowedRoles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var rolesSession = context.HttpContext.Session.GetString("roles");
            var roles = context.HttpContext.Session.GetString("roles")?.Split(',') ?? new string[0];

            // Check if the session variable "roles" exists
            if (rolesSession == null)
            {
                // Redirect to unauthorized page if no roles session exists
                context.Result = new RedirectToActionResult("Unauthenticated", "Error", null);
                return; // Exit the method after setting the result
            }


            if (!_allowedRoles.Any(role => roles.Contains(role)))
            {
                 //Redirect to a specific page if the role is not allowed
                context.Result = new RedirectToActionResult("Unauthorized", "Error", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
