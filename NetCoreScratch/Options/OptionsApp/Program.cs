using OptionsApp;
using OptionsLib;
using OptionsLib.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

//Setup DI Services
var services = new ServiceCollection();
services.AddSingleton<Application>();
services.AddLogging(options =>
{
    options.AddConsole();
});

//****DO DEFAULT OPTIONS
//services.AddDummy();

//****DO INLINE OPTIONS
//services.AddDummy(options =>
//{
//    options.Name = "Inline";
//    options.RandomSeed = 42;
//    options.MaxValue = 1000;
//    options.MinValue = 100;
//    options.OnNumberGenerated = number => Console.WriteLine($"Generated number: {number}");
//    options.ThrowException = true;
//});

//**** DO CONFIGURATION OPTIONS
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
services.AddDummy();
services.Configure<DummyOptions>(configuration.GetSection("DummyOptions"));

//Launch the app
var serviceProvider = services.BuildServiceProvider();
var application = serviceProvider.GetRequiredService<Application>();
await application.RunAsync();