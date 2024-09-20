using SwaggerAuth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization(); //Must be before Swagger middleware
app.UseMiddleware<SwaggerBasicAuthMiddleware>();    //Must be before swagger

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();