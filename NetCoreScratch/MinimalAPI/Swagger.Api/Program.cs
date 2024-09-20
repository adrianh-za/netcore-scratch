
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swagger.Api;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<PersonService>();
builder.Services.AddSingleton<GuidGenerator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //options.EnableAnnotations();
    options.OperationFilter<SwaggerUnboundParametersOperationFilter>();
});


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

//People
// app.MapGet("/people", (PersonService personService) => personService.GetPeople())
//     .WithTags("People API (USING WITHS())")          //The group of the endpoint
//     .WithDescription("Get People DESCRIPTION")       //The description of the endpoint
//     .WithSummary("Get People SUMMARY")               //The summary of the endpoint
//     .WithName("Get People NAME")                     //The name used internally when referencing the endpoint.  Not for Swagger
//     .WithDisplayName("Get People DISPLAY NAME").     //??

//WithOpenApi()
app.MapGet("/people-open/{value:int}",
    (int value, PersonService personService) =>
    {
        var person = personService.GetPerson(value);
        return person is not null ? Results.Ok(person) : Results.NotFound();
    })

    .AllowAnonymous()
    .Produces<List<Person>>()
    .ProducesProblem(StatusCodes.Status500InternalServerError)
    .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
    .WithOpenApi(operation =>
    {
        operation.Tags = [new OpenApiTag() { Name = "People API (using WithOpenOpen())" }];
        operation.Parameters.Single(c => c.Name == "value").Description = "The person ID";
        operation.Parameters.Single(c => c.Name == "value").Required = true;
        operation.Description = "Get People DESCRIPTION";
        operation.Summary = "Get People SUMMARY";
        operation.Responses[StatusCodes.Status200OK.ToString()].Description = "People fetched";
        operation.Responses[StatusCodes.Status422UnprocessableEntity.ToString()].Description = "Validation failed";
        operation.Responses[StatusCodes.Status500InternalServerError.ToString()].Description = "General error";
        return operation;
    });

//Annotations[]
app.MapGet("/people-anno/{value:int}",
    [Tags("People API (using Annotations[])")]
    [SwaggerOperation(Summary = "Get People SUMMARY", Description = "Get People DESCRIPTION")]
    [SwaggerUnboundParameter("value", "The person ID", true)]
    [SwaggerResponse((int)HttpStatusCode.NoContent, "No content.")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Not found")]
    (int value, PersonService personService) =>
{
    var person = personService.GetPerson(value);
    if (person is not null)
        return Results.Ok(person);

    return Results.NotFound();
});

//Doesn't work
// app.MapGet("/people/{value:int}", (int value, PersonService personService) =>
//     {
//         var person = personService.GetPerson(value);
//         if (person is not null)
//             return Results.Ok(person);
//
//         return Results.NotFound();
//     })
//     .WithMetadata(new SwaggerTagAttribute("People API (USING SWAGGER)"))
//     .WithMetadata(new SwaggerOperationAttribute("value", "The person ID"));


app.Run();