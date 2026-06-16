using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DoAn_QLTTT.Controllers;

public abstract class AdminControllerBase : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ViewBag.CurrentUserName = "Admin Demo";
        ViewBag.CurrentUserRole = "Admin";
        base.OnActionExecuting(context);
    }
}
