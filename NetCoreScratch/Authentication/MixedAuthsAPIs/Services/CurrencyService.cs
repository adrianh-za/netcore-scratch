namespace MixedAuthsAPIs.Data;

public class CurrencyService
{
    public static readonly Dictionary<string, double> Currencies = new()
    {
        { "EUR", 1.3d },
        { "GBP", 1.4d },
        { "USD", 1.2d },
        { "YEN", 0.2d },
        { "ZAR", 0.5d },
    };

    public async Task<Dictionary<string, double>> GetCurrencies(string baseCurrency)
    {
        var baseValue = Currencies[baseCurrency];
        var exchangeRates = new Dictionary<string, double>();

        foreach (var currency in Currencies)
        {
            exchangeRates[currency.Key] =  Math.Round(currency.Value / baseValue, 2);
        }

        return await Task.FromResult(exchangeRates);
    }
}