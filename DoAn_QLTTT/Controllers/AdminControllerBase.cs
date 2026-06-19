using DoAn_QLTTT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DoAn_QLTTT.Controllers;

[Authorize]
public abstract class AdminControllerBase : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var action = context.ActionDescriptor as ControllerActionDescriptor;
        var isDeleteAction = string.Equals(action?.ActionName, "Delete", StringComparison.OrdinalIgnoreCase)
            || string.Equals(action?.MethodInfo.Name, "DeleteConfirmed", StringComparison.OrdinalIgnoreCase);

        if (isDeleteAction
            && !User.IsInRole(AppStatuses.VaiTro.Admin))
        {
            context.Result = Forbid();
            return;
        }

        ViewBag.CurrentUserName = User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name ?? "";
        ViewBag.CurrentUserRole = User.FindFirstValue(ClaimTypes.Role) ?? "";
        base.OnActionExecuting(context);
    }
}
