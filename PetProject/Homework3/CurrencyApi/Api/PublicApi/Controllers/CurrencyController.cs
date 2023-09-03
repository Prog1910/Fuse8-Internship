using Application.Public.Interfaces.Rest;
using Contracts;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Controllers;

/// <summary>
///     Controller for handling currency-related operations.
/// </summary>
[ApiController]
[Route("currency-api/currency")]
public sealed class CurrencyController : ControllerBase
{
	private readonly IInternalApi _internalService;

	public CurrencyController(IInternalApi internalService)
	{
		_internalService = internalService;
	}

	/// <summary>
	///     Gets the current default currency exchange rate.
	/// </summary>
	/// <response code="200">The default currency exchange rate was successfully obtained.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="422">A validation error occurred while processing the request.</response>
	/// <response code="429">You have reached your rate or monthly limit for accessing this endpoint.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("")]
	[ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrentCurrency()
	{
		var currencyDto = await _internalService.GetCurrentCurrencyAsync();

		return Ok(currencyDto.Adapt<CurrencyResponse>());
	}

	/// <summary>
	///     Gets the current default currency exchange rate for the specified date.
	/// </summary>
	/// <param name="date">The date on which you want to get default currency exchange rate.</param>
	/// <response code="200">The default currency exchange rate was successfully obtained.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="422">A validation error occurred while processing the request.</response>
	/// <response code="429">You have reached your rate or monthly limit for accessing this endpoint.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("{date}")]
	[ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrencyOnDate(DateOnly date)
	{
		var currencyDto = await _internalService.GetCurrencyOnDateAsync(date);

		return Ok(currencyDto.Adapt<CurrencyResponse>());
	}

	/// <summary>
	///     Gets the current favorite currencies exchange rate.
	/// </summary>
	/// <param name="name">The name of favorite currencies you want to get.</param>
	/// <response code="200">The favorite currencies exchange rate was successfully obtained.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="422">A validation error occurred while processing the request.</response>
	/// <response code="429">You have reached your rate or monthly limit for accessing this endpoint.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("favorites/{name}")]
	[ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrentFavoritesByName(string name)
	{
		var currencyDto = await _internalService.GetCurrentFavoritesByNameAsync(name);

		return Ok(currencyDto.Adapt<CurrencyResponse>());
	}

	/// <summary>
	///     Gets the current favorite currencies exchange rate for the specified date.
	/// </summary>
	/// <param name="name">The name of favorite currencies you want to get.</param>
	/// <param name="date">The date on which you want to get favorite currencies exchange rate.</param>
	/// <response code="200">The favorite currencies exchange rate was successfully obtained.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="422">A validation error occurred while processing the request.</response>
	/// <response code="429">You have reached your rate or monthly limit for accessing this endpoint.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("favorites/{name}/{date}")]
	[ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetFavoritesOnDate(string name, DateOnly date)
	{
		var currencyDto = await _internalService.GetFavoritesOnDateByNameAsync(name, date);

		return Ok(currencyDto.Adapt<CurrencyResponse>());
	}

	/// <summary>
	///     Retrieves the API configuration settings.
	/// </summary>
	/// <response code="200">The settings was successfully obtained.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("settings")]
	[ProducesDefaultResponseType(typeof(SettingsResponse))]
	public async Task<IActionResult> GetSettings()
	{
		var settingsDto = await _internalService.GetSettingsAsync();

		return Ok(settingsDto.Adapt<SettingsResponse>());
	}
}