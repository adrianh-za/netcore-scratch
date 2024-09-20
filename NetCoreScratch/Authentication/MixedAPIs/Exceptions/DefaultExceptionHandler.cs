using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MixedAPIs.Exceptions;

public class DefaultExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {

        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsync("FROM DefaultExceptionHandler", cancellationToken);
        return true;
        /*
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(exception);

        //Determine the correct HTTP status code based on the exception type
        (int status, string title) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Not found."),
            BadRequestException => (StatusCodes.Status400BadRequest, "Bad request."),
            ApiVersionException => (StatusCodes.Status400BadRequest, "Api version error."),
            NotImplementedException => (StatusCodes.Status501NotImplemented, "Not implemented."),
            ConflictException => (StatusCodes.Status409Conflict, "Conflict error."),
            AuthenticationException => (StatusCodes.Status401Unauthorized, "Authentication error."),
            AuthorizationException => (StatusCodes.Status403Forbidden, "Authorization error."),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error has occurred."),
        };

        //Generate the ProblemDetails to be returned
        var problemDetails = new ProblemDetails()
        {
            Title = title,

            Status = status,
            Instance = $"{context.Request.Method} {context.Request.Path}",
            Type = exception.GetType().Name,
            Detail = exception.Message,
            Extensions = { ["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier }
        };

        //Return the response
        context.Response.StatusCode = problemDetails.Status.Value;
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;*/
    }
}