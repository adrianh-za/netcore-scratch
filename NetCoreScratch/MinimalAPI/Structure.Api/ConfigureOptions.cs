using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Structure.Api;

public class ConfigureSwaggerGenOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        //Add swagger document for every API version discovered
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
        }
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription desc)
    {
        var info = new OpenApiInfo()
        {
            Title = "Minimal API",
            Version = desc.ApiVersion.ToString()
        };

        if (desc.IsDeprecated)
            info.Description += " This API version has been deprecated. Please use one of the new APIs available from the explorer.";

        return info;
    }
}

public class ConfigureApiExplorerOptions : IConfigureNamedOptions<ApiExplorerOptions>
{
    public void Configure(ApiExplorerOptions options)
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    }

    public void Configure(string? name, ApiExplorerOptions options)
    {
        Configure(options);
    }
}

public class ConfigureApiVersioningOptions : IConfigureNamedOptions<ApiVersioningOptions>
{
    public void Configure(ApiVersioningOptions options)
    {
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
        options.ReportApiVersions = true;
    }

    public void Configure(string? name, ApiVersioningOptions options)
    {
        Configure(options);
    }
}