using Application.Common.Interfaces;
using Contracts;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Controllers;

/// <summary>
/// Controller for handling currency-related operations.
/// </summary>
[ApiController]
[Route("currencyapi")]
public sealed class CurrencyController : ControllerBase
{
	private readonly IInternalApi _internalService;
	private readonly IPublicApi _publicService;

	public CurrencyController(IInternalApi internalService, IPublicApi publicService)
	{
		_internalService = internalService;
		_publicService = publicService;
	}

	/// <summary>
	/// Gets the current exchange rate in the specified currency.
	/// </summary>
	/// <response code="200">The exchange rate for the requested currency was successfully obtained.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="422">A validation error occurred while processing the request.</response>
	/// <response code="429">You have reached your rate or monthly limit for accessing this endpoint.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("currency")]
	[ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrentCurrency()
	{
		var currencyResponse = await _internalService.GetCurrentCurrencyAsync();

		return Ok(currencyResponse);
	}

	/// <summary>
	/// Gets information about the currency for the specified date.
	/// </summary>
	/// <param name="date">Date on which you want to get information about the currency.</param>
	/// <response code="200">The exchange rate for the requested currency was successfully obtained.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="422">A validation error occurred while processing the request.</response>
	/// <response code="429">You have reached your rate or monthly limit for accessing this endpoint.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("currency/{date}")]
	[ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrencyOnDate(DateOnly date)
	{
		var currencyResponse = await _internalService.GetCurrencyOnDateAsync(date);

		return Ok(currencyResponse);
	}

	/// <summary>
	/// Retrieves the API configuration settings.
	/// </summary>
	/// <response code="200">The settings was successfully obtained.</response>
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

	/// <summary>
	/// Retrieves the API configuration settings.
	/// </summary>
	/// <response code="200">The default currency was successfully changed.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpPut("/settings/default-currency")]
	[ProducesDefaultResponseType(typeof(void))]
	public async Task<IActionResult> UpdateDefaultCurrency([FromQuery] CurrencyType defaultCurrency)
	{
		await _publicService.UpdateDefaultCurrencyAsync(defaultCurrency);

		return Accepted();
	}

	/// <summary>
	/// Retrieves the API configuration settings.
	/// </summary>
	/// <response code="200">The currency round count was successfully changed.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpPut("/settings/currency-round-count")]
	[ProducesDefaultResponseType(typeof(void))]
	public async Task<IActionResult> UpdateCurrencyRoundCount([FromQuery] int currencyRoundCount)
	{
		await _publicService.UpdateCurrencyRoundCountAsync(currencyRoundCount);

		return Accepted();
	}
}
