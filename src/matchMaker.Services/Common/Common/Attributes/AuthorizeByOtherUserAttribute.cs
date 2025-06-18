using Common.Authorization.Context;
using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.Attributes;

public class AuthorizeByOtherUserAttribute : Attribute, IActionFilter
{
    private readonly string _userIdProperty;

    public AuthorizeByOtherUserAttribute(string userIdProperty = "UserId")
    {
        _userIdProperty = userIdProperty;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var authContext = context.HttpContext.RequestServices.GetService(typeof(IAuthenticationContext)) as IAuthenticationContext;
        if (authContext == null || !authContext.IsUser)
            return;

        long? routeUserId = null;

        foreach (var arg in context.ActionArguments.Values)
        {
            if (arg == null) continue;

            if (arg is long idValue)
            {
                routeUserId = idValue;
                break;
            }

            var prop = arg.GetType().GetProperty(_userIdProperty);
            if (prop?.GetValue(arg) is long id)
            {
                routeUserId = id;
                break;
            }
        }

        if (routeUserId == null)
        {
            context.Result = new BadRequestObjectResult("Не удалось определить идентификатор пользователя");
            return;
        }

        if (authContext.UserId != routeUserId)
        {
            throw new ForbiddenException("Вы не можете выполнять действия от имени другого пользователя");
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}