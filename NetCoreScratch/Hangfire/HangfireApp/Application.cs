using Hangfire;
using Microsoft.Extensions.Logging;

namespace HangfireApp;

public class Application
{
    private readonly ILogger<Application> _logger;
    private bool _disposedValue;

    public Application(ILogger<Application> logger)
    {
        _logger = logger;
    }

    public async Task RunAsync()
    {
        await Task.Delay(1);

        using (var backgroundJobServer = new BackgroundJobServer())
        {
            RecurringJob.AddOrUpdate("RecurringJob1", () => DoRecurringJob1(), Cron.Minutely );
            BackgroundJob.Schedule(() => DoScheduledJob(), TimeSpan.FromSeconds(5));
            Console.ReadLine();
        }
    }

    public void DoRecurringJob1()
    {
        Console.WriteLine("Executed RecurringJob1");
        _logger.LogInformation("Executed RecurringJob1");
    }

    public void DoScheduledJob()
    {
        Console.WriteLine("Executed ScheduledJob");
        _logger.LogInformation("Executed ScheduledJob");
    }
}