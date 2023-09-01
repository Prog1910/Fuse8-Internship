using Application.Public.Interfaces.Rest;
using Application.Shared.Dtos;
using Contracts;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Controllers;

/// <summary>
///     Controller for handling currency-related operations.
/// </summary>
[ApiController]
[Route("currency-api")]
public sealed class CurrencyController : ControllerBase
{
	private readonly IInternalApi _internalService;
	private readonly IMapper _mapper;

	public CurrencyController(IInternalApi internalService, IMapper mapper)
	{
		_internalService = internalService;
		_mapper = mapper;
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
	[HttpGet("currency")]
	[ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrentCurrency()
	{
		var currencyDto = await _internalService.GetCurrentCurrencyAsync();
		var currencyResponse = _mapper.Map<CurrencyResponse>(currencyDto);

		return Ok(currencyResponse);
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
	[HttpGet("currency/{date}")]
	[ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrencyOnDate(DateOnly date)
	{
		var currencyDto = await _internalService.GetCurrencyOnDateAsync(date);
		var currencyResponse = _mapper.Map<CurrencyResponse>(currencyDto);

		return Ok(currencyResponse);
	}

	/// <summary>
	///     Gets the current favorite currency exchange rate.
	/// </summary>
	/// <param name="name">The name of favorite currency you want to get.</param>
	/// <response code="200">The favorite currency exchange rate was successfully obtained.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="422">A validation error occurred while processing the request.</response>
	/// <response code="429">You have reached your rate or monthly limit for accessing this endpoint.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("favorite-currency/{name}")]
	[ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrentFavoriteCurrencyByNameAsync(string name)
	{
		var currencyDto = await _internalService.GetCurrentFavoriteCurrencyByNameAsync(name);
		var currencyResponse = _mapper.Map<CurrencyResponse>(currencyDto);

		return Ok(currencyResponse);
	}

	/// <summary>
	///     Gets the current favorite currency exchange rate for the specified date.
	/// </summary>
	/// <param name="name">The name of favorite currency you want to get.</param>
	/// <param name="date">The date on which you want to get favorite currency exchange rate.</param>
	/// <response code="200">The favorite currency exchange rate was successfully obtained.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="422">A validation error occurred while processing the request.</response>
	/// <response code="429">You have reached your rate or monthly limit for accessing this endpoint.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("favorite-currency/{name}/{date}")]
	[ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetFavoriteCurrencyOnDateAsync(string name, DateOnly date)
	{
		var currencyDto = await _internalService.GetFavoriteCurrencyOnDateByNameAsync(name, date);
		var currencyResponse = _mapper.Map<CurrencyResponse>(currencyDto);

		return Ok(currencyResponse);
	}

	/// <summary>
	///     Retrieves the API configuration settings.
	/// </summary>
	/// <response code="200">The settings was successfully obtained.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("/settings")]
	[ProducesDefaultResponseType(typeof(SettingsResponse))]
	public async Task<IActionResult> GetSettings()
	{
		var fullSettingsDto = await _internalService.GetSettingsAsync();
		var settingsResponse = _mapper.Map<SettingsResponse>(fullSettingsDto);

		return Ok(settingsResponse);
	}
}