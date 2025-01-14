using Microsoft.ApplicationInsights.Channel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#if DEBUG
    Console.WriteLine("Debug mode");
#elif RELEASE
    Console.WriteLine("Release mode");
#else
    Console.WriteLine("Unknown mode");
#endif

//Setup configuration (NOTE: The order of configuration is important)
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var channel = new InMemoryChannel();

//Log to app insights
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConfiguration(configuration.GetSection("Logging"));
    builder.AddConsole();
    builder.AddApplicationInsights(
        configureTelemetryConfiguration: (config) =>
        {
            config.ConnectionString = configuration.GetValue("ApplicationInsights:ConnectionString", string.Empty);
            config.TelemetryChannel = channel;
        },
        configureApplicationInsightsLoggerOptions: (options) =>
        {
            options.IncludeScopes = true;
            options.FlushOnDispose = true;
        }
    );
});

//Setup DI Services
var services = new ServiceCollection();
services.AddScoped<IConfiguration>(_ => configuration);
services.AddSingleton(loggerFactory);
var serviceProvider = services.BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();
logger.LogDebug("DEBUG");
logger.LogInformation("INFORMATION");
logger.LogWarning("WARNING");

channel.Flush();
Thread.Sleep(5000); // Allow time for flushing