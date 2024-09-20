using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace MixedAuthsAPIs.Authorization;

public class IpAddressAuthHandler(IOptionsMonitor<IpAddressAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : AuthenticationHandler<IpAddressAuthOptions>(options, logger, encoder)
{
    private readonly string[] _ipWhitelist = ["1.2.3.4"];
    //private readonly string[] _ipWhitelist = ["1.2.3.4", "0.0.0.1"];

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var ipAddress = Context.Connection.RemoteIpAddress?.MapToIPv4().ToString();

        //IP address must exist
        if (string.IsNullOrWhiteSpace(ipAddress))
            return Task.FromResult(AuthenticateResult.Fail("Failed to retrieve the IP address"));

        //IP Address must be in the whitelist
        if (!_ipWhitelist.Contains(ipAddress))
            return Task.FromResult(AuthenticateResult.Fail("IP address not allowed"));

        //Add claim to the principal for IP Address Key
        var claims = new[] { new Claim("ip-address", ipAddress) };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}