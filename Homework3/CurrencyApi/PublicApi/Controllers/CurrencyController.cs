using Fuse8_ByteMinds.SummerSchool.PublicApi.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers;

[Route("currencyapi")]
public class CurrencyController : Controller
{
	private readonly CurrencyApiSettings _currencyServiceSettings;
	private readonly HttpClient _httpClient;

	public CurrencyController(IOptions<CurrencyApiSettings> options, HttpClient httpClient)
	{
		_currencyServiceSettings = options.Value;
		_httpClient = httpClient;
	}

	/// <summary>
	/// Latest Сurrency Exchange Data (default currency RUB, default base currency USD)
	/// </summary>
	/// <response code="200">
	/// Returns if it was possible to get the default currency data (default RUB).	
	/// </response>
	[HttpGet]
	[Route("currency")]
	public async Task<CurrencyData> GetLatestExchangeRates()
	{
		_httpClient.DefaultRequestHeaders.Add("apikey", _currencyServiceSettings.ApiKey);

		var requestUri = "https://api.currencyapi.com/v3/latest?currencies=RUB&base_currency=USD";

		var responseMessage = await _httpClient.GetAsync(requestUri);

		if (responseMessage.IsSuccessStatusCode)
		{
			var jsonDocument = JsonDocument.Parse(await responseMessage.Content.ReadAsStringAsync());
			var root = jsonDocument.RootElement;

			if (root.TryGetProperty("data", out var currencyData))
			{
				var currencyDatum = currencyData.EnumerateObject().FirstOrDefault();
				var code = currencyDatum.Value.GetProperty("code").GetString();
				var value = Math.Round(currencyDatum.Value.GetProperty("value").GetDecimal(), _currencyServiceSettings.CurrencyRoundCount);
				return new(code!, value);
			}
		}
		throw new NotImplementedException();
	}

	[HttpGet]
	[Route("settings")]
	public async Task<CurrentSettings> ChechApiStatus()
	{
		_httpClient.DefaultRequestHeaders.Add("apikey", _currencyServiceSettings.ApiKey);
		var requestUri = "https://api.currencyapi.com/v3/status";
		var responseMessage = await _httpClient.GetAsync(requestUri);
		if (responseMessage.IsSuccessStatusCode)
		{
			var jsonDocument = JsonDocument.Parse(await responseMessage.Content.ReadAsStringAsync());
			var root = jsonDocument.RootElement;

			if (root.TryGetProperty("quotas", out var quotas))
			{
				if (quotas.TryGetProperty("month", out var month))
				{
					var defaultCurrency = _currencyServiceSettings.DefaultCurrency;
					var baseCurrency = _currencyServiceSettings.BaseCurrency;
					var currencyRoundCount = _currencyServiceSettings.CurrencyRoundCount;
					var requestLimit = month.GetProperty("total").GetInt32();
					var requestCount = month.GetProperty("used").GetInt32();

					return new(defaultCurrency, baseCurrency, requestLimit, requestCount, currencyRoundCount);
				}
			}
		}
		throw new NotImplementedException();
	}
}

public record CurrencyData(string Code, decimal Value);

public record CurrentSettings(string DefaultCurrency, string BaseCurrency, int RequestLimit, int RequestCount, int CurrencyRoundCount);