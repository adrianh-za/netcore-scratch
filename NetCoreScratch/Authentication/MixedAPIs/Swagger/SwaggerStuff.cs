using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MixedAPIs;


/// <summary>
/// Controller conventions to control the visibility of a controller in the swagger documentation
/// </summary>
/// <param name="env"></param>
public class ApiExplorerVisibilityConvention(IWebHostEnvironment env) : IActionModelConvention
{
    public void Apply(ActionModel action)
    {
        //Try find the SwaggerVisibilityAttribute on the controller
        var attribute = action.Controller
            .Attributes
            .OfType<ApiExplorerVisibilityAttribute>()
            .SingleOrDefault();

        //Determine if the controller should be visible in the ApiExplorer (which is what swagger uses)
        var isVisible = attribute is null ||
                        attribute.Visibility == ApiExplorerVisibilityAttribute.Environment.All ||
                        attribute.Visibility == ApiExplorerVisibilityAttribute.Environment.Development && env.IsDevelopment();

        //Set visibility
        action.ApiExplorer.IsVisible = isVisible;
    }
}

/// <summary>
/// Attrubute to control the visibility of a controller in the swagger documentation
/// </summary>
/// <param name="env"></param>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ApiExplorerVisibilityAttribute(ApiExplorerVisibilityAttribute.Environment env) : Attribute
{
    public enum Environment
    {
        All,
        Development,
        None,
    }

    public Environment Visibility => env;
}

/*
// ReSharper disable once ClassNeverInstantiated.Global
/// <summary>
/// Document filter to remove controllers from the swagger documentation
/// </summary>
/// <param name="env"></param>
public class VisibilityDocumentFilter(IWebHostEnvironment env) : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        //Check each controller (api) in the context
        foreach (var api in context.ApiDescriptions)
        {
            //Try finding the SwaggerVisibilityAttribute on the controller (api)
            var attribute = api.CustomAttributes().OfType<ApiExplorerVisibilityAttribute>().FirstOrDefault();

            //Check the visibility conditions of the attribute
            if  (attribute is null ||
                attribute.Visibility == ApiExplorerVisibilityAttribute.Environment.All ||
                attribute.Visibility == ApiExplorerVisibilityAttribute.Environment.Development && env.IsDevelopment())
                continue;

            //Reached here, so controller (api) should be removed
            swaggerDoc.Paths.Remove("/" + api.RelativePath);
        }
    }
}
*/

public static class SwaggerGenOptionsExtension
{
    /// <summary>
    /// Add the JWT Bearer token authentication to the swagger documentation
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public static void AddJwtBearerTokenAuthentication(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Enter JWT token",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT", 
            Scheme = "bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    }

    /// <summary>
    /// Add the API Key authentication to the swagger documentation
    /// </summary>
    /// <param name="options"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static void AddApiKeyAuthentication(this SwaggerGenOptions options, string name = "x-api-key")
    {
        options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
        {
            Description = "Enter API Key",
            Name = name,
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
    }

    /// <summary>
    /// Add the Basic authentication to the swagger documentation
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public static void AddBasicAuthentication(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("BasicAuth", new OpenApiSecurityScheme
        {
            Description = "Enter username and password",
            Name = "Basic",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "basic"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="BasicAuth"
                    }
                },
                Array.Empty<string>()
            }
        });
    }
}