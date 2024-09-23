using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FormCRUDB_MAB
{
    // Action filter to check user access and prevent caching of pages
    public class CheckAccess : ActionFilterAttribute, IAuthorizationFilter
    {
        // Authorization check to ensure the user is logged in
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            // Check if UserID exists in session, otherwise redirect to login page
            if (filterContext.HttpContext.Session.GetString("UserID") == null)
            {
                filterContext.Result = new RedirectResult("~/Users/Login");
            }
        }

        // Prevent page caching to ensure the user cannot navigate back to cached pages after logout
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Expires"] = "-1";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";
            base.OnResultExecuting(context);
        }
    }
}
