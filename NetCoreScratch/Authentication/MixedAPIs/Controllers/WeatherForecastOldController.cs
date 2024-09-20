// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using MixedAPIs.Authorization;
// using MixedAPIs.Exceptions;
// using Swashbuckle.AspNetCore.Annotations;
//
// namespace MixedAPIs.Controllers;
//
// [ApiController]
// [Route("[controller]")]
// [ApiExplorerVisibility(ApiExplorerVisibilityAttribute.Environment.All)]
// [ServiceFilter(typeof(ExceptionHandlingAttribute))]
// [Authorize]
// public class WeatherForecastOldController : ControllerBase
// {
//     private static readonly string[] Summaries = new[]
//     {
//         "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//     };
//
//     private readonly ILogger<WeatherForecastOldController> _logger;
//
//     public WeatherForecastOldController(ILogger<WeatherForecastOldController> logger)
//     {
//         _logger = logger;
//     }
//
//     [HttpGet("day")]
//     [Authorize(AuthenticationSchemes = "ApiKeyAuth")]
//     [SwaggerOperation(Summary = "Gets the weather", Description = "Retrieves the weather forecast for the next 1 day.")]
//     public IEnumerable<WeatherForecast> GetDay()
//     {
//         return Enumerable.Range(1, 5).Select(index => new WeatherForecast
//             {
//                 Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                 TemperatureC = Random.Shared.Next(-20, 55),
//                 Summary = Summaries[Random.Shared.Next(Summaries.Length)]
//             })
//             .ToArray();
//     }
//     
//     [HttpGet("week")]
//     [Authorize(AuthenticationSchemes = "ApiKeyAuth", Policy = nameof(Policy.WeatherDayForecast) )]
//     [SwaggerOperation(Summary = "Gets the weather", Description = "Retrieves the weather forecast for the next 7 days.")]
//     public IEnumerable<WeatherForecast> GetWeek()
//     {
//         return Enumerable.Range(1, 7).Select(index => new WeatherForecast
//             {
//                 Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                 TemperatureC = Random.Shared.Next(-20, 55),
//                 Summary = Summaries[Random.Shared.Next(Summaries.Length)]
//             })
//             .ToArray();
//     }
//     
//     [HttpGet("month")]
//     [Authorize(AuthenticationSchemes = "ApiKeyAuth")]
//     [SwaggerOperation(Summary = "Gets the weather", Description = "Retrieves the weather forecast for the 28 days.")]
//     public IEnumerable<WeatherForecast> GetMonth()
//     {
//         return Enumerable.Range(1, 28).Select(index => new WeatherForecast
//             {
//                 Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                 TemperatureC = Random.Shared.Next(-20, 55),
//                 Summary = Summaries[Random.Shared.Next(Summaries.Length)]
//             })
//             .ToArray();
//     }
//     
//     [HttpGet("exception")]
//     [SwaggerOperation(Summary = "Gets the weather", Description = "Retrieves the weather forecast for the next 5 days.")]
//     public IEnumerable<WeatherForecast> ThrowException()
//     {
//         throw new Exception("This is an exception from WeatherForecastOldController");
//     }
// }
