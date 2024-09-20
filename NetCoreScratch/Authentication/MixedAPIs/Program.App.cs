using MixedAPIs.Exceptions;

namespace MixedAPIs;

public static class ProgramApps
{
    public static void Configure(WebApplication app)
    {
        //Swagger
        app.UseSwagger();
        app.UseSwaggerUI();

        //Routing stuff
        app.UseHttpsRedirection();
        app.UseRouting();

        //Auth stuff
        app.UseAuthentication();
        app.UseAuthorization();

        //MVC
        app.MapControllers();
        
        //Exception handlers
        app.UseExceptionHandler(options => { });    //NOTE: Options must be specified, even if empty
    }
}