using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

//Keep the namespace same as the standard Swagger namespace as this is merely an extension of that library
namespace Swagger.Api;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class SwaggerUnboundParameterAttribute : SwaggerParameterAttribute
{
    public SwaggerUnboundParameterAttribute(string name, string description, bool required) : base(description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException($"'{nameof(description)}' cannot be null or empty.", nameof(description));

        Name = name;
        base.Required = required;

        //This is just a hack as we can't update the base class (See property description)
        Required = required;
    }

    public string Name { get; set; }

    /*
     * This is needed because the base class SwaggerParameterAttribute didn't expose RequiredFlag to allow for inheritance, but calling Required GET throws an exception
     * telling you to access RequiredFlag to retrieve value.  RequiredFlag should be "protected internal" and not just "internal" so inheritance works correctly.
     */
    internal new bool Required { get; set; }
}

//Swagger OperationFilter for SwaggerUnboundParameterAttribute
public class SwaggerUnboundParametersOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(context);

        if (operation.Parameters is null)
            return;

        foreach (var customAttribute in context.MethodInfo.GetCustomAttributes<SwaggerUnboundParameterAttribute>())
        {
            //Try find the parameter for the current attribute
            var parameter = operation
                .Parameters
                .SingleOrDefault(c => c.Name == customAttribute.Name);

            if (parameter is null)
                continue;

            //Found the parameter specified
            parameter.Description = customAttribute.Description;
            parameter.Required = customAttribute.Required;
        }
    }
}