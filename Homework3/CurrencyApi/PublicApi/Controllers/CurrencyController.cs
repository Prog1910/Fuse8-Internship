using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Currency;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Status;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers;

/// <summary>
/// Methods for handling exchange rate conversion
/// </summary>
[Route("currencyapi")]
public class CurrencyController : ControllerBase
{
	private readonly CurrencyApiSettings _currencyServiceSettings;
	private readonly HttpClient _httpClient;

	public CurrencyController(IOptionsSnapshot<CurrencyApiSettings> options, HttpClient httpClient)
	{
		_currencyServiceSettings = options.Value;
		_httpClient = httpClient;
		ConfigureRequestHeaders();
	}

	#region GET /currency
	/// <summary>
	/// Latest Default Сurrency Exchange Rate (default currency RUB, default base currency USD)
	/// </summary>
	/// <response code="200">
	/// Default currency rate was received successfully
	/// </response>
	/// <response code="403">
	/// You are not allowed to use this endpoint
	/// </response>
	/// <response code="404">
	/// Requested endpoint does not exist
	/// </response>
	/// <response code="422">
	/// Validation error
	/// </response>
	/// <response code="429">
	/// You have hit your rate limit or your monthly limit
	/// </response>
	/// <response code="500">
	/// Internal server error 
	/// </response>
	[HttpGet("currency")]
	public async Task<CurrencyDataDto> GetCurrencyExchangeRate()
	{
		var requestUri = $"https://api.currencyapi.com/v3/latest?currencies={_currencyServiceSettings.DefaultCurrency}&base_currency={_currencyServiceSettings.BaseCurrency}";
		var responseMessage = await _httpClient.GetAsync(requestUri);
		if (responseMessage.IsSuccessStatusCode)
		{
			var currencyResponse = await DeserializeResponse<CurrencyResponseDto>(responseMessage);
			if (currencyResponse is not null)
			{
				var currencyData = currencyResponse.CurrenciesData[_currencyServiceSettings.DefaultCurrency];
				return new(currencyData.Code, Math.Round(currencyData.Value, _currencyServiceSettings.CurrencyRoundCount));
			}
		}
		throw new HttpRequestException("Failed to get default currency data", null, HttpStatusCode.InternalServerError);
	}
	#endregion

	#region GET /currency/{currencyCode}
	/// <summary>
	/// Latest Сurrency Exchange Rate (default base currency USD)
	/// </summary>
	/// <param name="currencyCode">Currency whose current rate will be returned (format: upper case)</param>
	/// <response code="200">
	/// Currency rate was received successfully
	/// </response>
	/// <response code="403">
	/// You are not allowed to use this endpoint
	/// </response>
	/// <response code="404">
	/// Requested endpoint does not exist
	/// </response>
	/// <response code="422">
	/// Validation error
	/// </response>
	/// <response code="429">
	/// You have hit your rate limit or your monthly limit
	/// </response>
	/// <response code="500">
	/// Internal server error 
	/// </response>
	[HttpGet("currency/{currencyCode}")]
	public async Task<CurrencyDataDto> GetCurrencyExchangeRateByCode(string currencyCode)
	{
		var requestUri = $"https://api.currencyapi.com/v3/latest?currencies={currencyCode}&base_currency={_currencyServiceSettings.BaseCurrency}";
		var responseMessage = await _httpClient.GetAsync(requestUri);
		if (responseMessage.IsSuccessStatusCode)
		{
			var currencyResponse = await DeserializeResponse<CurrencyResponseDto>(responseMessage);
			if (currencyResponse is not null)
			{
				var currencyData = currencyResponse.CurrenciesData[currencyCode];
				return new(currencyData.Code, Math.Round(currencyData.Value, _currencyServiceSettings.CurrencyRoundCount));
			}
		}
		throw new HttpRequestException("Failed to get default currency data", null, HttpStatusCode.InternalServerError);
	}
	#endregion

	#region GET /currency/{currencyCode}/{date}
	/// <summary>
	/// Historical Сurrency Exchange Rate (default base currency USD)
	/// </summary>
	/// <param name="date">Date to retrieve historical rates from (format: yyyy-MM-dd)</param>
	/// <param name="currencyCode">Currency whose current rate will be returned (format: upper case)</param>
	/// <response code="200">
	/// Historical currency rate was received successfully
	/// </response>
	/// <response code="403">
	/// You are not allowed to use this endpoint
	/// </response>
	/// <response code="404">
	/// Requested endpoint does not exist
	/// </response>
	/// <response code="422">
	/// Validation error
	/// </response>
	/// <response code="429">
	/// You have hit your rate limit or your monthly limit
	/// </response>
	/// <response code="500">
	/// Internal server error 
	/// </response>
	[HttpGet("currency/{currencyCode}/{date}")]
	public async Task<HistoricalCurrencyDataDto> GetHistoricalCurrencyExchangeRate(string currencyCode, string date)
	{
		var requestUri = $"https://api.currencyapi.com/v3/historical?currencies={currencyCode}&date={date}&base_currency={_currencyServiceSettings.BaseCurrency}";
		var responseMessage = await _httpClient.GetAsync(requestUri);
		if (responseMessage.IsSuccessStatusCode)
		{
			var currencyResponse = await DeserializeResponse<CurrencyResponseDto>(responseMessage);
			if (currencyResponse is not null)
			{
				var currencyData = currencyResponse.CurrenciesData[currencyCode];
				return new(date, currencyData.Code, Math.Round(currencyData.Value, _currencyServiceSettings.CurrencyRoundCount));
			}
		}
		throw new HttpRequestException("Failed to get default currency data", null, HttpStatusCode.InternalServerError);
	}
	#endregion

	#region GET /settings
	/// <summary>
	/// Current Quota Information
	/// </summary>
	/// <response code="200">
	/// API Status was received successfully
	/// </response>
	/// <response code="404">
	/// Requested endpoint does not exist
	/// </response>
	/// <response code="500">
	/// Internal server error 
	/// </response>
	[HttpGet("settings")]
	public async Task<CurrentStatusDto> GetApiSettings()
	{
		var requestUri = "https://api.currencyapi.com/v3/status";
		var responseMessage = await _httpClient.GetAsync(requestUri);
		if (responseMessage.IsSuccessStatusCode)
		{
			var apiStatus = await DeserializeResponse<ApiStatusDto>(responseMessage);
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
	#endregion

	#region Helper Methods
	private void ConfigureRequestHeaders()
		=> _httpClient.DefaultRequestHeaders.Add("apikey", _currencyServiceSettings.ApiKey);

	private async Task<TDto?> DeserializeResponse<TDto>(HttpResponseMessage responseMessage)
		=> JsonSerializer.Deserialize<TDto>(await responseMessage.Content.ReadAsStringAsync());
	#endregion
}
