using Asp.Versioning;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;

namespace Structure.Api;

public static class PeopleEndpointsV1
{
    public static void MapPeopleEndpointsV1(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .HasApiVersion(new ApiVersion(2, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup("api/v{version:apiVersion}/people")
            .RequireCors("AllowAll")    //Set the CORS policy
            .WithTags("People API")
            .WithApiVersionSet(versionSet)
            .HasApiVersion(new ApiVersion(1, 0));

        group.MapGet("/typed-results", GetPeopleTypedResults)
            .WithName("GetPeople-TypesResults")
            .WithOpenApi(operation =>
            {
                operation.Description = "Get People DESCRIPTION";
                operation.Summary = "Get People SUMMARY";
                return operation;
            });

        group.MapGet("/iresult", GetPeopleIResult)
            .WithName("GetPeople-IResult")
            .WithOpenApi(operation =>
            {
                operation.Description = "Get People DESCRIPTION";
                operation.Summary = "Get People SUMMARY";
                return operation;
            });

        group.MapGet("/produces", GetPeopleIResult)
            .WithName("GetPeople-Produces")
            .Produces<List<Person>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .WithOpenApi(operation =>
            {
                operation.Description = "Get People DESCRIPTION";
                operation.Summary = "Get People SUMMARY";
                return operation;
            });

        group.MapGet("/openapi", GetPeopleIResult)
            .WithName("GetPeople-OpenApi")
            .WithOpenApi(operation =>
            {
                operation.Description = "Get People DESCRIPTION";
                operation.Summary = "Get People SUMMARY";
                //operation.Responses.Add(StatusCodes.Status200OK.ToString(), new OpenApiResponse { Description = "People fetched" });
                operation.Responses.Add(StatusCodes.Status422UnprocessableEntity.ToString(), new OpenApiResponse { Description = "Validation failed" });
                operation.Responses.Add(StatusCodes.Status500InternalServerError.ToString(), new OpenApiResponse { Description = "General error" });
                return operation;
            });

        group.MapGet("/mixed", GetPeopleIResult)
            .WithName("GetPeople-Mixed")
            .Produces<List<Person>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .WithOpenApi(operation =>
            {
                operation.Description = "Get People DESCRIPTION";
                operation.Summary = "Get People SUMMARY";
                operation.Responses[StatusCodes.Status200OK.ToString()].Description = "People fetched";
                operation.Responses[StatusCodes.Status422UnprocessableEntity.ToString()].Description = "Validation failed";
                operation.Responses[StatusCodes.Status500InternalServerError.ToString()].Description = "General error";
                return operation;
            });

        group.MapGet("/search", SearchPeople)
            .WithName("SearchPeople")
            .WithOpenApi(operation =>
            {
                operation.Parameters.Single(c => c.Name == "term").Description = "search term";
                operation.Parameters.Single(c => c.Name == "term").Required = true;
                operation.Description = "Search People DESCRIPTION";
                operation.Summary = "Search People SUMMARY";

                return operation;
            });



        group.MapGet("{id}", GetPerson)
            .WithName("GetPerson");
    }

    public static Results<Ok<List<Person>>, BadRequest<string>> GetPeopleTypedResults(PersonService personService)
    {
        try
        {
            var people =  personService.GetPeople();
            return TypedResults.Ok(people);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest("ERROR");
        }
    }

    public static IResult GetPeopleIResult(PersonService personService)
    {
        try
        {
            var people =  personService.GetPeople();
            return TypedResults.Ok(people);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest("ERROR");
        }
    }



    public static Results<Ok<Person>, NotFound<string>> GetPerson(PersonService personService, int personId)
    {
        var person = personService.GetPerson(personId);
        if (person is null)
            return TypedResults.NotFound($"Person not found for ID = {personId}");

        return TypedResults.Ok(person);
    }

    public static IResult SearchPeople(PersonService personService, string term)
    {
        var people = personService.GetPeople(term);
        return Results.Ok(people);
    }
}

/*

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
});*/