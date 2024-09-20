using System.Security.Claims;
using System.Text;
using Basic.Api;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<PersonService>();
builder.Services.AddSingleton<GuidGenerator>();
var app = builder.Build();

//Simple routes
var route = "/";
app.MapGet(route, () => "Result from GET request");
app.MapPost(route, () => "Result from POST request");
app.MapPut(route, () => "Result from PUT request");
app.MapDelete(route, () => "Result from DELETE request");
app.MapMethods(route, new[] { "HEAD", "PATCH" }, () => "Result from HEAD or PATCH request");

//Routes with params
var routeValue = "/{value:int}";
app.MapGet(routeValue, (int value) => $"Result from GET request with value '{value}'");
app.MapPost(routeValue, (int value) => $"Result from POST request with value '{value}'");
app.MapPut(routeValue, (int value) => $"Result from PUT request with value '{value}'");
app.MapDelete(routeValue, (int value) => $"Result from DELETE request with value '{value}'");
app.MapMethods(routeValue, new[] { "HEAD", "PATCH" }, (int value) => $"Result from from HEAD or PATCH request with value '{value}'");

//Routes with local functions
var routeLocal = "/local/{value:int}";
string GetMessage(int value, string method) => $"Result from {method} request with value '{value}'";
app.MapGet(routeLocal, (int value) => GetMessage(value, HttpMethods.Get));
app.MapPost(routeLocal, (int value) => GetMessage(value, HttpMethods.Post));
app.MapPut(routeLocal, (int value) => GetMessage(value, HttpMethods.Put));
app.MapDelete(routeLocal, (int value) => GetMessage(value, HttpMethods.Delete));
app.MapMethods(routeLocal, new[] { HttpMethods.Options, HttpMethods.Patch }, (int value)
    => GetMessage(value, HttpMethods.Patch + " or " + HttpMethods.Options));

//Routes with delegate functions
var routeDelegate = "/delegate/{value:int}";
var handler = (int value, string method) =>  $"Result from {method} request with value '{value}'";
app.MapGet(routeDelegate, (int value) => handler(value, HttpMethods.Get));
app.MapPost(routeDelegate, (int value) => handler(value, HttpMethods.Post));
app.MapPut(routeDelegate, (int value) => handler(value, HttpMethods.Put));
app.MapDelete(routeDelegate, (int value) => handler(value, HttpMethods.Delete));
app.MapMethods(routeDelegate, new[] { HttpMethods.Options, HttpMethods.Patch }, (int value)
    => handler(value, HttpMethods.Patch + " or " + HttpMethods.Options));

//Routes with query params
//EG /mixed-spec/11?term=doe
app.MapGet("/mixed/{value:int}", (int value, string term, GuidGenerator guidGenerator) =>
{
    return $"{value} {term} {guidGenerator.GenerateGuid()}";
});
//EG /mixed-spec1/11?term=doe
app.MapGet("/mixed-spec1/{value:int}",
    (
        [FromRoute]int value,
        [FromQuery]string term,
        [FromServices]GuidGenerator guidGenerator) =>
    {
        return $"{value} {term} {guidGenerator.GenerateGuid()}";
    }
);
//EG /mixed-spec2/11?query=doe
app.MapGet("/mixed-spec2/{value:int}",
    (
        [FromRoute]int value,
        [FromQuery(Name = "query")]string term,
        [FromServices]GuidGenerator guidGenerator,
        [FromHeader(Name = "User-Agent")] string userAgent) =>
    {
        return $"{value} {term} {userAgent} {guidGenerator.GenerateGuid()}";
    }
);

//People
app.MapGet("/people", (PersonService personService) => personService.GetPeople());
app.MapGet("/people/{value:int}", (int value, PersonService personService) =>
{
    var person = personService.GetPerson(value);
    if (person is not null)
        return Results.Ok(person);

    return Results.NotFound();
});
app.MapDelete("/people/{value:int}", (int value, PersonService personService) =>
{
    personService.RemovePerson(value);
    return Results.NoContent();
});
app.MapGet("/people/search", (string term, PersonService personService) =>
{
    var people = personService.GetPeople(term);
    return Results.Ok(people);
});
app.MapPost("/people", (Person person, PersonService personService) =>
{
    personService.AddPerson(person);
    return Results.NoContent();
});

//Special
app.MapGet("context-agent", (HttpContext context) =>
{
    var userAgent = context.Request.Headers["User-Agent"];
    return Results.Ok(userAgent);
});
app.MapGet("request-agent", async(HttpRequest request, HttpResponse response) =>
{
    var userAgent = request.Headers["User-Agent"];
    await response.WriteAsync(userAgent);
});
app.MapGet("request-queries", async(HttpRequest request, HttpResponse response) =>
{
    var queries = request.QueryString.Value;
    await response.WriteAsync(queries);
});
app.MapGet("claims", (ClaimsPrincipal user) =>
{
    var auth = user.Identity.IsAuthenticated ? "Authenticated" : "Not Authenticated";
    return Results.Ok(auth);
});

//Returns
app.MapGet("/string", () => "String result from GET request");
app.MapGet("/json-raw", () => new { message = "Json result from GET request"});
app.MapGet("/ok-string", () => Results.Ok("String result from GET request"));
app.MapGet("/json-obj", () => Results.Json(new { message = "Json result from GET request"}));
app.MapGet("/text-string", () => Results.Text("String result from GET request"));
app.MapGet("/stream", () =>
{
    var stream = new MemoryStream(Encoding.UTF8.GetBytes("Stream result from GET request"));
    stream.Seek(0, SeekOrigin.Begin);
    return Results.Stream(stream, "text/plain");
});
app.MapGet("/redirect", () => Results.Redirect("www.google.com"));
app.MapGet("/download", () => Results.File("./myfile.txt", "text/plain"));


//Result Extension
app.MapGet("html", () =>
{
    return Results.Extensions.Html("<html><body><h1>Html result from GET request</h1></body></html>");
});


app.Run();