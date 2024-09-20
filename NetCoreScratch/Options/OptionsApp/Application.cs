using OptionsLib.Interfaces;

namespace OptionsApp;

public class Application
{
    private readonly IDummyService _dummyService;

    public Application(IDummyService dummyService)
    {
        _dummyService = dummyService;
    }

    public async Task RunAsync()
    {
        var random = await _dummyService.GetRandomNumber();
        Console.WriteLine(random);
    }
}
