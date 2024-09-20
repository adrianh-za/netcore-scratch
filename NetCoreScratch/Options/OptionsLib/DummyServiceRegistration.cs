using Microsoft.Extensions.DependencyInjection;
using OptionsLib.Interfaces;
using OptionsLib.Options;
using OptionsLib.Services;

namespace OptionsLib;

public static class DummyServiceRegistration
{
    public static IServiceCollection AddDummy(this IServiceCollection services, Action<DummyOptions>? options = null)
    {
        if (options is not null)
            services.Configure(options);

        services.AddScoped<IDummyService, DummyService>();
        services.AddLogging();
        return new ServiceCollection();
    }
}
