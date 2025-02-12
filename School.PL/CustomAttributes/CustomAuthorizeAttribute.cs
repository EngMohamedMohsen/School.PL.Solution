using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using School.PL.Services;
using School.DAL.Models;

namespace School.PL.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizeAttribute : Attribute
    {
        UserServices userServices;
        UserManager<AppUser> userManager;
        private readonly string RoleName;

        public CustomAuthorizeAttribute(string roleName)
        {
            RoleName = roleName;
        }

        public async Task<string> IsUserInRoleAsync(Guid id, string roleName)
        {
            var user = await userServices.GetUserByIdAsync(id); // Await the task to get the user object.

            if (user == null)
            {
                return "User not found."; // No user is found
            }

            // Check if the user is in the specified role
            var isInRole = await userManager.IsInRoleAsync(user, roleName);
            if (isInRole)
            {
                return roleName; // If the user is in the role, return the role name.
            }
            else
            {
                return "User is not in the specified role."; // If the user is not in the role, return this message.
            }
        }
    }
}
#region MyRegion
//public async Task<string> IsUserInRoleAsync(Guid id, string roleName)
//{
//    var user = await _userManager.FindByIdAsync(id.ToString()); // Await the task to get the user object.

//    if (user == null)
//    {
//        return "User not found."; // No user is found
//    }

//    // Check if the user is in the specified role
//    bool isInRole = await _userManager.IsInRoleAsync(user, roleName);
//    if (isInRole)
//    {
//        return roleName; // If the user is in the role, return the role name.
//    }
//    else
//    {
//        return "User is not in the specified role."; // If the user is not in the role, return this message.
//    }
//} 
#endregion
#region MyRegion
//public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
//{
//    private readonly string _RoleName;

//    public CustomAuthorizeAttribute(string roleName)
//    {
//        _RoleName = roleName;
//    }

//    public void OnAuthorization(AuthorizationFilterContext context)
//    {
//        var user = context.HttpContext.User;

//        // Check if the user is authenticated
//        if (!user.Identity?.IsAuthenticated ?? true)
//        {
//            return;
//        }

//        // Check if the user has the required role
//        if (!user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _RoleName))
//        {
//            context.Result = new ForbidResult();
//        }
//    }
//} 
#endregion