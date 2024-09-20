using Microsoft.AspNetCore.Authentication;

namespace MixedAuthsAPIs.Authorization;

public class IpAddressAuthOptions : AuthenticationSchemeOptions
{
    public string IpAddress { get; set; } = string.Empty;
}