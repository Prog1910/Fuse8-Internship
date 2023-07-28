using Fuse8_ByteMinds.SummerSchool.PublicApi.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers;

[Route("currency")]
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
	/// Latest Currency Exchange Data
	/// </summary>
	/// <response code="200">
	/// This endpoint returns currency exchange data for any given base currency (default USD).	
	/// </response>
	[HttpGet]
	public async Task<CurrencyData> GetCurrencyRate()
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
				var value = Math.Round(currencyDatum.Value.GetProperty("value").GetDecimal(), _currencyServiceSettings.DecimalPlaces);
				return new(code!, value);
			}
		}
		throw new NotImplementedException();
	}
}

public record CurrencyData(string Code, decimal Value);
