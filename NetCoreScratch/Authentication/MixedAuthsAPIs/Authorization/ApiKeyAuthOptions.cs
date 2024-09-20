using Microsoft.AspNetCore.Authentication;

namespace MixedAuthsAPIs.Authorization;

public class ApiKeyAuthOptions : AuthenticationSchemeOptions
{
    public string ApiKey { get; set; } = string.Empty;
}