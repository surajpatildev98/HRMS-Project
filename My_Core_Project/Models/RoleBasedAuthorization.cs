using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace My_Core_Project.Models
{
    public class RoleBasedAuthorization : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public RoleBasedAuthorization(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // 🔹 1. User logged in check
            if (!user.Identity.IsAuthenticated)
            {
                HandleUnauthorized(context, "Please log in to access this action.");
                return;
            }

            // 🔹 2. Get user role from claims
            var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // 🔹 3. Role check
            if (string.IsNullOrEmpty(role) || !_roles.Contains(role))
            {
                HandleUnauthorized(context, "You do not have permission to perform this action.");
                return;
            }

            // 🔹 4. Set IsAdmin cookie for UI usage
            var isAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;
            context.HttpContext.Response.Cookies.Append(
                "IsAdmin",
                isAdmin == "True" ? "True" : "False"
            );
        }

        /// <summary>
        /// Handle unauthorized requests — returns JSON for AJAX, View for normal requests
        /// </summary>
        private void HandleUnauthorized(AuthorizationFilterContext context, string message)
        {
            bool isAjax = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjax)
            {
                // 🔹 Return JSON for AJAX calls
                context.Result = new JsonResult(new { success = false, message })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
            else
            {
                // 🔹 Return custom AccessDenied view for normal requests
                context.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/AccessDenied.cshtml",
                    ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                        new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                        context.ModelState
                    )
                    {
                        { "Message", message }
                    }
                };
            }
        }
    }
}
