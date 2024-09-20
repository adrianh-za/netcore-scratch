using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MixedAuthsAPIs.Data;

namespace MixedAuthsAPIs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrencyController(CurrencyService currencyService) : ControllerBase
{
    [HttpGet("auth-anon")]
    [AllowAnonymous]
    public async Task<ActionResult<Dictionary<string, double>>> GetCurrencies_AuthAnon(string baseCurrency = "USD")
    {
        var currencies = await currencyService.GetCurrencies(baseCurrency);
        return Ok(currencies);
    }

    [HttpGet("auth-separate")]
    [Authorize(AuthenticationSchemes = "ApiKeyAuth")]
    [Authorize(AuthenticationSchemes = "IpAddressAuth")]
    public async Task<ActionResult<Dictionary<string, double>>> GetCurrencies_AuthSeparate(string baseCurrency = "USD")
    {
        var currencies = await currencyService.GetCurrencies(baseCurrency);
        return Ok(currencies);
    }

    [HttpGet("auth-policy")]
    [Authorize(Policy = "Combined")]
    public async Task<ActionResult<Dictionary<string, double>>> GetCurrencies_AuthPolicy(string baseCurrency = "USD")
    {
        var currencies = await currencyService.GetCurrencies(baseCurrency);
        return Ok(currencies);
    }

}