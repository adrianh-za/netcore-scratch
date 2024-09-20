using MixedAPIs;

//Configure the services
var builder = WebApplication.CreateBuilder(args);
ProgramServices.Configure(builder);

//Configure the app
var app = builder.Build();
ProgramApps.Configure(app);

//Run the app
app.Run();