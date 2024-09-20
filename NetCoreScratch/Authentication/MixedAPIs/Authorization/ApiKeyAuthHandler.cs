using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace MixedAPIs.Authorization;

public class ApiKeyAuthHandler(IOptionsMonitor<ApiKeyAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : AuthenticationHandler<ApiKeyAuthOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var apiKey = Context.Request.Headers["x-api-key"].FirstOrDefault();
        if (String.IsNullOrWhiteSpace(apiKey))
            return Task.FromResult(AuthenticateResult.Fail("API Key is missing."));
        
        var claims = new[] { new Claim("api-key", apiKey) };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}