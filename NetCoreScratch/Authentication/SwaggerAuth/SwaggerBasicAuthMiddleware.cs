using System.Net;

namespace SwaggerAuth;

public class SwaggerBasicAuthMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        //Handle swagger requests only
        if (!context.Request.Path.StartsWithSegments("/swagger"))
        {
            await next.Invoke(context);
            return;
        }

        //Fetch the BASIC authorization header
        var authHeaders = context.Request.Headers.Authorization.ToArray();
        var basicAuthHeader = authHeaders.FirstOrDefault(head => head is "Basic");
        if (string.IsNullOrWhiteSpace(basicAuthHeader))
        {

            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.Headers["WWW-Authenticate"] = "Basic"; //Force browser to show login

            //await next.Invoke(context);
            return;
        }

        //Process the BASIC authorization header
        var username = basicAuthHeader!.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
        var password = basicAuthHeader!.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[2]?.Trim();

        //TODO: Do some checking, but ignore for now.
        Console.WriteLine($"Username: {username}");
        Console.WriteLine($"Password: {password}");

        //All good, allow through
        await next.Invoke(context);
    }
}