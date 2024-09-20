using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MixedAPIs.Authorization;

/// <summary>
/// AuthorizationRequirement for role (Roles Enum)
/// </summary>
public class RoleRequirement : IAuthorizationRequirement
{
    public IEnumerable<Roles> Roles { get; init; }

    public RoleRequirement(IEnumerable<Roles> roles)
    {
        Roles = roles;
    }
}

/// <summary>
/// Handles the RoleRequirement by checking if the current user has role (Roles Enum) assigned in database
/// and return Success (authorized) else Fail (unauthorized)
/// </summary>
public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        //Check if user is authenticated
        if (context.User.Identity?.IsAuthenticated != true)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        //Check if authenticated user has role claim
        var hasClaim = context
            .User
            .HasClaim(claim => claim.Type == ClaimTypes.Role && requirement.Roles.Any(role => role.ToString() == claim.Value));

        //Set result and return
        if (hasClaim)
            context.Succeed(requirement);
        else
            context.Fail();

        return Task.CompletedTask;
    }
}