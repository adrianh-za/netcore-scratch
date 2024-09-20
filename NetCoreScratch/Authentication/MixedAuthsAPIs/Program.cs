using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using MixedAuthsAPIs.Authorization;
using MixedAuthsAPIs.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Enter API Key",
        Name = "x-api-key",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });

});
builder.Services.AddRouting(options => options.LowercaseUrls = true);

//****** Add stuff ******
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddAuthentication()
    .AddScheme<IpAddressAuthOptions, IpAddressAuthHandler>("IpAddressAuth", null)
    .AddScheme<ApiKeyAuthOptions, ApiKeyAuthHandler>("ApiKeyAuth", null);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Combined", policy =>
    {
        policy.AddRequirements(new ApiKeyAuthRequirement(), new IpAddressAuthRequirement());
        policy.AddAuthenticationSchemes("IpAddressAuth", "ApiKeyAuth");
    });
});
builder.Services.AddTransient<IAuthorizationHandler, IpAddressAuthRequirementHandler>();
builder.Services.AddTransient<IAuthorizationHandler, ApiKeyAuthRequirementHandler>();
//***********************

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

//****** Add stuff ******
app.UseAuthentication();    //Must occur before UseAuthorization()
app.UseAuthorization();
//***********************

app.MapControllers();
app.Run();