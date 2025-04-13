using Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
            return;

        var errors = context.ModelState
            .Where(kvp => kvp.Value?.Errors.Count > 0)
            .SelectMany(kvp => kvp.Value!.Errors.Select(error =>
                new ValidationError(kvp.Key, error.ErrorMessage)))
            .ToList();

        throw new ValidationException(errors);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        
    }
}