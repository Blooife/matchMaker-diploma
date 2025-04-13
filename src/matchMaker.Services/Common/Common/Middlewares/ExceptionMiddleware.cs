using System.Data.Common;
using System.Net;
using Common.Exceptions;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Minio.Exceptions;
using Newtonsoft.Json;

namespace Common.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
        
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
        
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
        
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        HttpStatusCode statusCode;
        string result;

        switch (exception)
        {
            case ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                result = CreatValidationErrorResponse(validationException.Errors, "ValidationError");
                break;
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                result = CreateErrorResponse(notFoundException.Message, "NotFoundError");
                break;
            case AlreadyContainsException alreadyContainsException:
                statusCode = HttpStatusCode.Conflict;
                result = CreateErrorResponse(alreadyContainsException.Message, "AlreadyContainsError");
                break;
            case NotContainsException notContainsException:
                statusCode = HttpStatusCode.NotFound;
                result = CreateErrorResponse(notContainsException.Message, "NotContainsError");
                break;
            case DbException dbException:
                statusCode = HttpStatusCode.BadRequest;
                result = CreateDbErrorResponse(dbException);
                break;
            case DbUpdateException dbUpdateException:
                statusCode = HttpStatusCode.BadRequest;
                result = CreateDbUpdateErrorResponse(dbUpdateException);
                break;
            case MinioException minioException:
                statusCode = HttpStatusCode.Conflict;
                result = CreateErrorResponse(minioException.Message, "MinioError");
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError;
                result = CreateErrorResponse(exception.Message, "Failure");
                break;
        }

        context.Response.StatusCode = (int)statusCode;
            
        return context.Response.WriteAsync(result);
    }
    
    private string CreateErrorResponse(string message, string errorType)
    {
        return JsonConvert.SerializeObject(new ErrorDetails
        {
            ErrorMessage = message,
            ErrorType = errorType
        });
    }
    
    private string CreatValidationErrorResponse(List<ValidationError> errors, string errorType)
    {
        var errorMessage = $"Validation Errors: ";

        foreach (var error in errors)
        {
            errorMessage += $" | {error.Error}";
        }
            
        return CreateErrorResponse(errorMessage, errorType);
    }

    private string CreateDbErrorResponse(DbException dbException)
    {
        var errorMessage = $"Database Error: {dbException.Message}";
        if (dbException.InnerException != null)
        {
            errorMessage += $" | Inner Exception: {dbException.InnerException.Message}";
        }
        //_logger.LogError(dbException, errorMessage);
            
        return CreateErrorResponse(errorMessage, "DatabaseError");
    }

    private string CreateDbUpdateErrorResponse(DbUpdateException dbUpdateException)
    {
        var errorMessage = $"Database Update Error: {dbUpdateException.Message}";
        if (dbUpdateException.InnerException != null)
        {
            errorMessage += $" | Inner Exception: {dbUpdateException.InnerException.Message}";
        }

        foreach (var entry in dbUpdateException.Entries)
        {
            errorMessage += $" | Entity: {entry.Entity.GetType().Name}, State: {entry.State}";
        }
            
        //_logger.LogError(dbUpdateException, errorMessage);
            
        return CreateErrorResponse(errorMessage, "DatabaseUpdateError");
    }
}