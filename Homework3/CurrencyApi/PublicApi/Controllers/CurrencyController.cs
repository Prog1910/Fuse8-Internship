using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Currency;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Status;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
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
	/// Latest Сurrency Exchange Data (default currency RUB, default base currency USD).
	/// </summary>
	/// <response code="200">
	/// Returns if it was possible to get the default currency data.	
	/// </response>
	/// <response code="500">
	/// Returns if the default currency data could not be retrieved.
	/// </response>
	[HttpGet]
	[Route("currency")]
	public async Task<CurrencyDataDto> GetLatestExchangeRates()
	{
		_httpClient.DefaultRequestHeaders.Add("apikey", _currencyServiceSettings.ApiKey);
		var responseMessage = await _httpClient.GetAsync("https://api.currencyapi.com/v3/latest?currencies=RUB&base_currency=USD");
		if (responseMessage.IsSuccessStatusCode)
		{
			var responseContent = await responseMessage.Content.ReadAsStringAsync();
			var currencyResponse = JsonSerializer.Deserialize<CurrencyResponseDto>(responseContent);
			if (currencyResponse is not null)
			{
				var currencyData = currencyResponse.CurrenciesData[_currencyServiceSettings.DefaultCurrency];
				return new(currencyData.Code, Math.Round(currencyData.Value, _currencyServiceSettings.CurrencyRoundCount));
			}
		}
		throw new HttpRequestException("Failed to get default currency data", null, HttpStatusCode.InternalServerError);
	}

	/// <summary>
	/// The status endpoint returns information about your current quota.
	/// </summary>
	/// <response code="200">
	/// Returns if it was possible to get the information about your current quota.	
	/// </response>
	/// <response code="500">
	/// Returns if the information about your current quota could not be retrieved.
	/// </response>
	[HttpGet]
	[Route("settings")]
	public async Task<CurrentStatusDto> ChechApiStatus()
	{
		_httpClient.DefaultRequestHeaders.Add("apikey", _currencyServiceSettings.ApiKey);
		var requestUri = "https://api.currencyapi.com/v3/status";
		var responseMessage = await _httpClient.GetAsync(requestUri);
		if (responseMessage.IsSuccessStatusCode)
		{
			var responseContent = await responseMessage.Content.ReadAsStringAsync();
			var apiStatus = JsonSerializer.Deserialize<ApiStatusDto>(responseContent);
			if (apiStatus is not null)
			{
				var month = apiStatus.Quotas.Month;
				return new(_currencyServiceSettings.DefaultCurrency,
					_currencyServiceSettings.BaseCurrency,
					month.Total,
					month.Used,
					_currencyServiceSettings.CurrencyRoundCount);
			}
		}
		throw new HttpRequestException("Failed to get current settings", null, HttpStatusCode.InternalServerError);
	}
}
