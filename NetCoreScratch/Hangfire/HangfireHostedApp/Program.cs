using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using HangfireHostedApp;
using Hangfire;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => {
        services.AddSingleton<Application>();
        services.AddHangfire(config =>
        {
            config.UseSimpleAssemblyNameTypeSerializer();
            config.UseRecommendedSerializerSettings();
            config.UseInMemoryStorage();
        });
        services.AddHangfireServer(options =>
        {
            options.SchedulePollingInterval = TimeSpan.FromSeconds(1);
        });
        services.AddLogging(options =>
        {
            options.AddConsole();
        });
    })
    .Build();

//Start the host
await host.StartAsync();

//Process the jobs
var application = host.Services.GetRequiredService<Application>();
await application.RunAsync();

//Wait around until app is killed
Console.ReadKey();