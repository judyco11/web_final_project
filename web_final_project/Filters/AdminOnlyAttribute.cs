using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace web_final_project.Filters
{
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAdmin = context.HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "True")
            {
                context.Result = new RedirectToActionResult("AccessDenied", "User", null);
            }
        }
    }
}

