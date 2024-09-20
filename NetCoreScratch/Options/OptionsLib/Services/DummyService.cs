using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OptionsLib.Interfaces;
using OptionsLib.Options;

namespace OptionsLib.Services;

public class DummyService : IDummyService
{
    private readonly ILogger<DummyService> _logger;
    private readonly IOptions<DummyOptions> _options;

    public DummyService(ILogger<DummyService> logger, IOptions<DummyOptions> options)
    {
        _logger = logger;
        _options = options;

        //Dump the current options
        var dummyOptionsJson = JsonSerializer.Serialize<DummyOptions>(_options.Value, new JsonSerializerOptions { WriteIndented = true });
        _logger.LogInformation("DummyOptions: {dummyOptionsJson}", dummyOptionsJson);
    }

    public async Task<int> GetRandomNumber()
    {
        //Get the min and max values from the options
        TimeSpan delay = _options.Value.Delay;
        int minValue = _options.Value.MinValue;
        int maxValue = _options.Value.MaxValue;
        int randomSeed = _options.Value.RandomSeed;

        await Task.Delay(delay);

        return await Task.FromResult(new Random(randomSeed).Next(minValue, maxValue));
    }
}
