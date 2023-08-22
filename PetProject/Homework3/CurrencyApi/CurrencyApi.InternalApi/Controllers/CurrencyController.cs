using CurrencyApi.Application.Common.Services.Interfaces;
using CurrencyApi.Protos;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyApi.InternalApi.Controllers;

/// <summary>
/// Controller for handling currency-related operations.
/// </summary>
[ApiController]
[Route("currencyapi")]
public sealed class CurrencyController : ControllerBase
{
	private readonly ICachedCurrencyApi _currencyService;
	private readonly IMapper _mapper;

	public CurrencyController(ICachedCurrencyApi currencyService, IMapper mapper)
	{
		_currencyService = currencyService;
		_mapper = mapper;
	}

	/// <summary>
	/// Gets the current exchange rate in the specified currency.
	/// </summary>
	/// <param name="currencyType">Type of currency for which you want to get the current rate.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <response code="200">The exchange rate for the requested currency was successfully obtained.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="422">A validation error occurred while processing the request.</response>
	/// <response code="429">You have reached your rate or monthly limit for accessing this endpoint.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("currencies")]
	[ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrentCurrency([FromQuery] Domain.Enums.CurrencyType currencyType, CancellationToken cancellationToken)
	{
		var currencyDto = await _currencyService.GetCurrentCurrencyAsync(currencyType, cancellationToken);

		return Ok(_mapper.Map<CurrencyResponse>(currencyDto));
	}

	/// <summary>
	/// Gets information about the currency for the specified date.
	/// </summary>
	/// <param name="currencyType">Type of currency for which you want to get the current rate.</param>
	/// <param name="date">Date on which you want to get information about the currency.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <response code="200">The exchange rate for the requested currency was successfully obtained.</response>
	/// <response code="403">You do not have permission to access this endpoint.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="422">A validation error occurred while processing the request.</response>
	/// <response code="429">You have reached your rate or monthly limit for accessing this endpoint.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("currencies/{date}")]
	[ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrencyOnDate([FromQuery] Domain.Enums.CurrencyType currencyType, DateOnly date, CancellationToken cancellationToken)
	{
		var currencyDto = await _currencyService.GetCurrencyOnDateAsync(currencyType, date, cancellationToken);

		return Ok(_mapper.Map<CurrencyResponse>(currencyDto));
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
	public async Task<IActionResult> GetSettings(CancellationToken cancellationToken)
	{
		var settingsDto = await _currencyService.GetSettingsAsync(cancellationToken);

		return Ok(_mapper.Map<SettingsResponse>(settingsDto));
	}
}