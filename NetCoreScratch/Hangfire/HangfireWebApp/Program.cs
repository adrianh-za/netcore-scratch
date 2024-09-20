using Hangfire;
using HangfireWebApp;

//***** Configure services
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging(options => options.AddConsole());
builder.Services.AddSingleton<Jobs>();

//Configure Hangfire
builder.Services.AddHangfire(configuration => configuration
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseInMemoryStorage());
builder.Services.AddHangfireServer();

//***** Configure app
var app = builder.Build();
app.UseHttpsRedirection();
app.UseHangfireDashboard();
app.MapHangfireDashboard();

//Schedule the jobs
var jobs = app.Services.GetRequiredService<Jobs>();
await jobs.RunAsync();

//Start the hosted app
app.Run();