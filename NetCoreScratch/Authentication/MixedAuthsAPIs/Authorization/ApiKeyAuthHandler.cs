using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace MixedAuthsAPIs.Authorization;

public class ApiKeyAuthHandler(IOptionsMonitor<ApiKeyAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : AuthenticationHandler<ApiKeyAuthOptions>(options, logger, encoder)
{
    private readonly string[] _apiKeys = ["12345", "67890"];

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        //API key must exist
        var apiKey = Context.Request.Headers["x-api-key"].FirstOrDefault();
        if (String.IsNullOrWhiteSpace(apiKey))
            return Task.FromResult(AuthenticateResult.Fail("Failed to retrieve the API Key"));

        //API Key must be in the whitelist
        if (!_apiKeys.Contains(apiKey))
            return Task.FromResult(AuthenticateResult.Fail("API Key not allowed"));

        //Add claim to the principal for API Key
        var claims = new[] { new Claim("api-key", apiKey) };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}