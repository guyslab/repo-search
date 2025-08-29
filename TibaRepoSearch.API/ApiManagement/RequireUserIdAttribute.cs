using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TibaRepoSearch;

public class RequireUserIdAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var requestContext = context.HttpContext.RequestServices.GetRequiredService<IRequestContext>();
        
        if (string.IsNullOrEmpty(requestContext.GetUserId()))
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        base.OnActionExecuting(context);
    }
}