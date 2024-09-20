
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Structure.Api;


//Order
// 1 Add API versioning
// 2 Add OpenAPI
// 3 Build service provider
// 4 Register Minimal APIs
// 5 Configure OpenAPI using DescribeApiVersions() to describe API versions


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<PersonService>();
builder.Services.AddSingleton<GuidGenerator>();

//Update JSON options
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.IncludeFields = true;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

//CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });

    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

//Configure API Versioning
builder.Services.ConfigureOptions<ConfigureApiVersioningOptions>();
builder.Services.ConfigureOptions<ConfigureApiExplorerOptions>();
builder.Services.AddEndpointsApiExplorer(); //Needed for minimal APIs
builder.Services.AddApiVersioning().AddApiExplorer();

//Configure Swagger
builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//CORS
app.UseCors();

//Add the endpoints
app.MapPeopleEndpointsV1();
app.MapPeopleEndpointsV2();

//Use Swagger
app.UseSwagger();

//https://github.com/dotnet/aspnet-api-versioning/issues/834
//https://github.com/dotnet/aspnet-api-versioning/blob/main/examples/AspNetCore/WebApi/MinimalOpenApiExample/Program.cs#L281
app.UseSwaggerUI(options =>
{
    //Create a swagger.json per version (can be moved outside program.cs, just pass in the app.DescribeApiVersions() to the method)
    app.DescribeApiVersions().ToList().ForEach(desc =>
    {
        options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
    });
});


//Run the app
app.Run();