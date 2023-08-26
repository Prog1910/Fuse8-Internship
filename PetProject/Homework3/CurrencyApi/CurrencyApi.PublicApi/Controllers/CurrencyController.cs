using CurrencyApi.Application.Common.Interfaces;
using CurrencyApi.Contracts;
using CurrencyApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyApi.PublicApi.Controllers;

/// <summary>
/// Controller for handling currency-related operations.
/// </summary>
[ApiController]
[Route("currencyapi")]
public sealed class CurrencyController : ControllerBase
{
	private readonly IInternalApi _internalService;

	public CurrencyController(IInternalApi internalService)
	{
		_internalService = internalService;
	}

	/// <summary>
	/// Gets the current exchange rate in the specified currency.
	/// </summary>
	/// <param name="currencyType">Type of currency for which you want to get the current rate.</param>
	/// <response code="200">The exchange rate for the requested currency was successfully obtained.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="422">A validation error occurred while processing the request.</response>
	/// <response code="429">You have reached your rate or monthly limit for accessing this endpoint.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("currencies")]
	[ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrentCurrency([FromQuery] CurrencyType currencyType)
	{
		var currencyResponse = await _internalService.GetCurrentCurrencyAsync(currencyType);

		return Ok(currencyResponse);
	}

	/// <summary>
	/// Gets information about the currency for the specified date.
	/// </summary>
	/// <param name="currencyType">Type of currency for which you want to get the current rate.</param>
	/// <param name="date">Date on which you want to get information about the currency.</param>
	/// <response code="200">The exchange rate for the requested currency was successfully obtained.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="422">A validation error occurred while processing the request.</response>
	/// <response code="429">You have reached your rate or monthly limit for accessing this endpoint.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("currencies/{date}")]
	[ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrencyOnDate([FromQuery] CurrencyType currencyType, DateOnly date)
	{
		var currencyResponse = await _internalService.GetCurrencyOnDateAsync(currencyType, date);

		return Ok(currencyResponse);
	}

	/// <summary>
	/// Retrieves the API configuration settings.
	/// </summary>
	/// <response code="200">The exchange rate for the requested currency was successfully obtained.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("/settings")]
	[ProducesDefaultResponseType(typeof(SettingsResponse))]
	public async Task<IActionResult> GetSettings()
	{
		var settingsResponse = await _internalService.GetSettingsAsync();

		return Ok(settingsResponse);
	}
}