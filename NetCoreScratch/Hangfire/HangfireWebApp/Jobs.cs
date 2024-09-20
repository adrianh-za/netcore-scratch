using Hangfire;

namespace HangfireWebApp;

public class Jobs
{
    private readonly ILogger<Jobs> _logger;

    public Jobs(ILogger<Jobs> logger)
    {
        _logger = logger;
    }

    public async Task RunAsync()
    {
        await Task.Delay(1);

        RecurringJob.AddOrUpdate("RecurringJob1", () => DoRecurringJob1(), Cron.Minutely );
        BackgroundJob.Schedule(() => DoScheduledJob(), TimeSpan.FromSeconds(5));
    }

    public void DoRecurringJob1()
    {
        _logger.LogInformation("Executed RecurringJob1");
    }

    public void DoScheduledJob()
    {
        _logger.LogInformation("Executed ScheduledJob");
    }
}
