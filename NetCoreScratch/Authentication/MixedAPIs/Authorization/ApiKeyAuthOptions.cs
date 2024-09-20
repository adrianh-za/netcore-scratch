using Microsoft.AspNetCore.Authentication;

namespace MixedAPIs.Authorization;

public class ApiKeyAuthOptions : AuthenticationSchemeOptions
{
    public string ApiKey { get; set; } = string.Empty;
}