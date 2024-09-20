using Microsoft.AspNetCore.Authorization;

namespace MixedAuthsAPIs.Authorization;

public class ApiKeyAuthRequirement : IAuthorizationRequirement
{
    public string ApiKey { get; set; } = string.Empty;
}

public class ApiKeyAuthRequirementHandler : AuthorizationHandler<ApiKeyAuthRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyAuthRequirement requirement)
    {
        //Check if user is authenticated
        if (context.User.Identity?.IsAuthenticated != true)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        //Find the expected claim
        var claim = context.User.FindFirst("api-key");
        if (claim is null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        //Claim exists
        requirement.ApiKey = claim.Value;
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}