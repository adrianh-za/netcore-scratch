using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MixedAPIs.Authorization;
using MixedAPIs.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace MixedAPIs.Controllers;

[ApiController]
[Route("[controller]")]
[ApiExplorerVisibility(ApiExplorerVisibilityAttribute.Environment.All)]
[Authorize]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet("weather-anon-week")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Gets the weather", Description = "Retrieves the weather forecast for the week.")]
    public IEnumerable<WeatherForecast> GetForecast_Anon()
    {
        return Enumerable.Range(1, 7).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [HttpGet("weather-bearer-week")]
    [Authorize]
    [SwaggerOperation(Summary = "Gets the weather", Description = "Retrieves the weather forecast for the week.")]
    public IEnumerable<WeatherForecast> GetForecast_Bearer()
    {
        return Enumerable.Range(1, 7).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [HttpGet("weather-authscheme-week")]
    [Authorize(AuthenticationSchemes = "ApiKeyAuth")]
    [SwaggerOperation(Summary = "Gets the weather", Description = "Retrieves the weather forecast for the week.")]
    public IEnumerable<WeatherForecast> GetForecast_AuthScheme()
    {
        return Enumerable.Range(1, 7).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [HttpGet("weather-policy_day")]
    [Authorize(Policy = nameof(Policy.WeatherDayForecast) )]
    [SwaggerOperation(Summary = "Gets the weather", Description = "Retrieves the weather forecast for the week.")]
    public IEnumerable<WeatherForecast> GetForecast_Policy_Guest()
    {
        return Enumerable.Range(1,1).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [HttpGet("weather-policy_week")]
    [Authorize(Policy = nameof(Policy.WeatherWeekForecast) )]
    [SwaggerOperation(Summary = "Gets the weather", Description = "Retrieves the weather forecast for the week.")]
    public IEnumerable<WeatherForecast> GetForecast_Policy_Admin()
    {
        return Enumerable.Range(1,7).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [HttpGet("weather-policy-auth-day")]
    [Authorize(AuthenticationSchemes = "ApiKeyAuth", Policy = nameof(Policy.WeatherDayForecast) )]
    [SwaggerOperation(Summary = "Gets the weather", Description = "Retrieves the weather forecast for the week.")]
    public IEnumerable<WeatherForecast> GetForecast_PolicyAuth_Guest()
    {
        return Enumerable.Range(1,1).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [HttpGet("weather-policy-auth-week")]
    [Authorize(AuthenticationSchemes = "ApiKeyAuth", Policy = nameof(Policy.WeatherWeekForecast) )]
    [SwaggerOperation(Summary = "Gets the weather", Description = "Retrieves the weather forecast for the week.")]
    public IEnumerable<WeatherForecast> GetForecast_PolicyAuth_Admin()
    {
        return Enumerable.Range(1,7).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
  
    [HttpGet("exception-handler")]
    [SwaggerOperation(Summary = "Throw Exception", Description = "Exception to be caught by the exception handler.")]
    [AllowAnonymous]
    public IEnumerable<WeatherForecast> ThrowException_Handler()
    {
        throw new Exception("This is an exception");
    }

    [HttpGet("exception-attribute")]
    [ServiceFilter(typeof(ExceptionHandlingAttribute))]
    [SwaggerOperation(Summary = "Throw Exception", Description = "Exception to be caught by the exception attribute.")]
    [AllowAnonymous]
    public IEnumerable<WeatherForecast> ThrowException_Attribute()
    {
        throw new Exception("This is an exception");
    }
}