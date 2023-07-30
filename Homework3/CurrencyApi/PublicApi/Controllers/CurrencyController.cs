using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Currency;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Status;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Extensions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
	public async Task<IActionResult> GetCurrencyExchangeRate()
	{
		var requestUri = $"https://api.currencyapi.com/v3/latest?currencies={_currencyServiceSettings.DefaultCurrency}&base_currency={_currencyServiceSettings.BaseCurrency}";
		var response = await _httpClient.GetAsync(requestUri);
		var currencyInfo = await response.EnsureValidAndDeserialize<CurrencyInfoDto>();
		var data = currencyInfo.Data[_currencyServiceSettings.DefaultCurrency];
		return Ok(new CurrencyDataDto(Code: data.Code, Value: RoundValue(data.Value)));
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
	public async Task<IActionResult> GetCurrencyExchangeRateByCode(string currencyCode)
	{
		var requestUri = $"https://api.currencyapi.com/v3/latest?currencies={currencyCode}&base_currency={_currencyServiceSettings.BaseCurrency}";
		var responseMessage = await _httpClient.GetAsync(requestUri);
		var currencyInfo = await responseMessage.EnsureValidAndDeserialize<CurrencyInfoDto>();
		var data = currencyInfo.Data[currencyCode];
		return Ok(new CurrencyDataDto(Code: data.Code, Value: RoundValue(data.Value)));
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
	public async Task<IActionResult> GetHistoricalCurrencyExchangeRate(string currencyCode, string date)
	{
		var requestUri = $"https://api.currencyapi.com/v3/historical?currencies={currencyCode}&date={date}&base_currency={_currencyServiceSettings.BaseCurrency}";
		var response = await _httpClient.GetAsync(requestUri);
		var currencyInfo = await response.EnsureValidAndDeserialize<CurrencyInfoDto>();
		var data = currencyInfo.Data[currencyCode];
		return Ok(new HistoricalCurrencyDataDto(date, data.Code, RoundValue(data.Value)));
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
	public async Task<IActionResult> GetApiSettings()
	{
		var requestUri = "https://api.currencyapi.com/v3/status";
		var responseMessage = await _httpClient.GetAsync(requestUri);
		var quotaInfo = await responseMessage.EnsureValidAndDeserialize<QuotaInfoDto>();
		var month = quotaInfo.Quotas.Month;
		return Ok(new CurrentStatusDto(DefaultCurrency: _currencyServiceSettings.DefaultCurrency,
			BaseCurrency: _currencyServiceSettings.BaseCurrency,
			RequestLimit: month.Total,
			RequestCount: month.Used,
			CurrencyRoundCount: _currencyServiceSettings.CurrencyRoundCount
			));
	}
	#endregion

	#region Helper Methods
	private void ConfigureRequestHeaders()
		=> _httpClient.DefaultRequestHeaders.Add("apikey", _currencyServiceSettings.ApiKey);

	private decimal RoundValue(decimal value)
		=> Math.Round(value, _currencyServiceSettings.CurrencyRoundCount);
	#endregion
}
