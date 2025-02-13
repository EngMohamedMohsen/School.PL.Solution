using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace School.PL.Helper.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string RoleName;

        public CustomAuthorizeAttribute(string roleName)
        {
            RoleName = roleName;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;

            // Check if user is authenticated
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("SignIn", "Account", null);
                return;
            }

            // Check if the user has the required role
            if (!user.IsInRole(RoleName))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                return;
            }

            // Proceed with the action execution
            await next();
        }
    }
}