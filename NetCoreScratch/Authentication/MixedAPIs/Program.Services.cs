using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MixedAPIs.Authorization;
using MixedAPIs.Exceptions;

namespace MixedAPIs;

public static class ProgramServices
{
    public static void Configure(WebApplicationBuilder builder)
    {
        //MVC
        builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(new ApiExplorerVisibilityConvention(builder.Environment));
        });

        //Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.AddBasicAuthentication();
            options.AddApiKeyAuthentication();
            options.AddJwtBearerTokenAuthentication();
        });

        //Authentication (Use http://jwtbuilder.jamiekurtz.com to generate JWT tokens)
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddScheme<ApiKeyAuthOptions, ApiKeyAuthHandler>("ApiKeyAuth", null)
        .AddJwtBearer(options =>
        {

            var key = "TheSuperSecretKeyThatIsNotSoSecret";
            var audience = "http://localhost";
            var issuer = "http://localhost";
            
            //options.Authority = "https://localhost:5044";     //Must be valid/reachable URL else it slows the calls down
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),  //NEVER DO THIS IN PRODUCTION....EVER!!!
                ValidateIssuerSigningKey = true,    //Reject invalid signing key
                ValidateLifetime = true,            //Reject expired keys
                ValidateAudience = true,            //Reject invalid audience
                ValidateIssuer = true,              //Reject invalid issuer
                ValidAudience = audience,
                ValidIssuer = issuer,
            };
        });
        
        //Authorization (Add each policy)
        foreach (var policyName in Enum.GetNames<Policy>())
        {
            var policy = (Policy)Enum.Parse(typeof(Policy), policyName);
            builder.Services.AddAuthorization(options => Policies.GetPolicy(options, policy));
        }
        builder.Services.AddTransient<IAuthorizationHandler, RoleRequirementHandler>();

        //Exception handling
        builder.Services.AddScoped<ExceptionHandlingAttribute>();
        builder.Services.AddExceptionHandler<DefaultExceptionHandler>();
        
        //Routing stuff
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
    }
}