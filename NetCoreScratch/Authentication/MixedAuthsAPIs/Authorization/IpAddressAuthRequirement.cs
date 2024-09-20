using Microsoft.AspNetCore.Authorization;

namespace MixedAuthsAPIs.Authorization;

public class IpAddressAuthRequirement : IAuthorizationRequirement
{
    public string IpAddress { get; set; } = string.Empty;
}

public class IpAddressAuthRequirementHandler : AuthorizationHandler<IpAddressAuthRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IpAddressAuthRequirement requirement)
    {
        //Check if user is authenticated
        if (context.User.Identity?.IsAuthenticated != true)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        //Find the expected claim
        var claim = context.User.FindFirst("ip-address");
        if (claim is null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        //Claim exists
        requirement.IpAddress = claim.Value;
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}