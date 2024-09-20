using Hangfire;
using Microsoft.Extensions.Logging;

namespace HangfireHostedApp;

public class Application: IDisposable
{
    private readonly BackgroundJobServer _backgroundJobServer;
    private readonly ILogger<Application> _logger;
    private bool _disposedValue;

    public Application(ILogger<Application> logger)
    {
        _backgroundJobServer = new BackgroundJobServer();
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

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _backgroundJobServer.Dispose();
            }
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
