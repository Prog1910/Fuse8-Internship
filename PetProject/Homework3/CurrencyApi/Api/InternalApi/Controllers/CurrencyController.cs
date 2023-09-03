using Application.Internal.Interfaces.Rest;
using Contracts;
using Contracts.Enums;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace InternalApi.Controllers;

/// <summary>
///     Controller for handling currency-related operations.
/// </summary>
[ApiController]
[Route("currency-api")]
public sealed class CurrencyController : ControllerBase
{
	private readonly ICacheCurrencyApi _currencyService;
	private readonly IRestApi _restService;

	public CurrencyController(ICacheCurrencyApi currencyService, IRestApi restService)
	{
		_currencyService = currencyService;
		_restService = restService;
	}

	/// <summary>
	///     Gets the current exchange rate in the specified currency.
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
	public async Task<IActionResult> GetCurrentCurrency([FromQuery] CurrencyType currencyType, CancellationToken cancellationToken)
	{
		var currencyDto = await _currencyService.GetCurrentCurrencyAsync((Domain.Enums.CurrencyType)currencyType, cancellationToken);

		return Ok(currencyDto.Adapt<CurrencyResponse>());
	}

	/// <summary>
	///     Gets information about the currency for the specified date.
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
	public async Task<IActionResult> GetCurrencyOnDate([FromQuery] CurrencyType currencyType, DateOnly date, CancellationToken cancellationToken)
	{
		var currencyDto = await _currencyService.GetCurrencyOnDateAsync((Domain.Enums.CurrencyType)currencyType, date, cancellationToken);

		return Ok(currencyDto.Adapt<CurrencyResponse>());
	}

	/// <summary>
	///     Retrieves the API configuration settings.
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

		return Ok(settingsDto.Adapt<SettingsResponse>());
	}

	/// <summary>
	///     Recalculates cached currency exchange rates relative to the specified base currency.
	/// </summary>
	/// <param name="baseCurrency">The base currency for which exchange rates should be recalculated.</param>
	/// <response code="200">Currencies was successfully recalculated.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpPost("/recalculation")]
	[ProducesDefaultResponseType(typeof(Guid))]
	public async Task<AcceptedResult> RecalculateCurrencyCacheA([FromQuery] CurrencyType baseCurrency)
	{
		var taskId = await _restService.RecalculateCurrencyCacheAsync((Domain.Enums.CurrencyType)baseCurrency);

		return Accepted(taskId);
	}
}