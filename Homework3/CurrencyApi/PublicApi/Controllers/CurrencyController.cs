using Fuse8_ByteMinds.SummerSchool.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers;

/// <summary>
/// Methods for handling exchange rate conversion
/// </summary>
[ApiController]
[Route("currencyapi")]
public class CurrencyController : ControllerBase
{
	private readonly ICurrencyService _currencyService;

	public CurrencyController(ICurrencyService currencyService)
	{
		_currencyService = currencyService;
	}

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
		var currencyData = await _currencyService.GetDefaultExchangeRate();
		return Ok(currencyData);
	}

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
		var currencyData = await _currencyService.GetExchangeRateByCode(currencyCode);
		return Ok(currencyData);
	}

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
		var currencyData = await _currencyService.GetHistoricalExchangeRateByCode(currencyCode, date);
		return Ok(currencyData);
	}

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
	public async Task<IActionResult> GetStatus()
	{
		var status = await _currencyService.GetStatus();
		return Ok(status);
	}
}
