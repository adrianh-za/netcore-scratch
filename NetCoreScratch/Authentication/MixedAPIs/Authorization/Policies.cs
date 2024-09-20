using Microsoft.AspNetCore.Authorization;

namespace MixedAPIs.Authorization;

//These are actions that can be performed by a user
public enum Policy
{
    WeatherDayForecast,
    WeatherWeekForecast
}


//These are roles that can be assigned to a user
public enum Roles
{
    Administrator,
    Guest
}

public static class Policies
{
    public static void GetPolicy(AuthorizationOptions options, Policy policy)
    {
        ArgumentNullException.ThrowIfNull(options);

        var policyName = policy.ToString();

        switch (policy)
        {
            case Policy.WeatherDayForecast:
                options.AddPolicy(policyName, policyConfig =>
                    policyConfig.Requirements.Add(new RoleRequirement(new[] {  Roles.Administrator, Roles.Guest })));
                break;

            case Policy.WeatherWeekForecast:
                options.AddPolicy(policyName, policyConfig =>
                    policyConfig.Requirements.Add(new RoleRequirement(new[] { Roles.Administrator })));
                break;

            default:
                throw new NotImplementedException($"Policy {policyName} not implemented.");
        }
    }
}