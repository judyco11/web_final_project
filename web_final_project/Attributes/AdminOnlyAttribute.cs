using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AdminOnlyAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new RedirectToRouteResult(
                new { controller = "Account", action = "Login" });
            return;
        }

        if (!user.IsInRole("Admin"))
        {
            context.Result = new RedirectToRouteResult(
                new { controller = "Home", action = "Index" });
        }
    }
}

