using Hangfire;
using HangfireApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

//Setup DI Services
var services = new ServiceCollection();
services.AddSingleton<Application>();
services.AddLogging(options =>
{
    options.AddConsole();
});

//NOTE: We can't use DI for non-hosted apps
Hangfire.GlobalConfiguration.Configuration.UseSimpleAssemblyNameTypeSerializer();
Hangfire.GlobalConfiguration.Configuration.UseRecommendedSerializerSettings();
Hangfire.GlobalConfiguration.Configuration.UseInMemoryStorage();

//Launch the app
var serviceProvider = services.BuildServiceProvider();
var application = serviceProvider.GetRequiredService<Application>();
await application.RunAsync();

//Wait around until app is killed
Console.ReadKey();