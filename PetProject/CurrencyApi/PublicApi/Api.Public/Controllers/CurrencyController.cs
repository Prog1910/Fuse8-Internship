using Application.Public.Interfaces.Rest;
using Application.Shared.Dtos;
using Contracts;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Controllers;

/// <summary> Controller for handling currency-related operations. </summary>
[ApiController, Route("currency-api/currency")]
public sealed class CurrencyController : ControllerBase
{
	private readonly IInternalApi _internalService;

	public CurrencyController(IInternalApi internalService)
	{
		_internalService = internalService;
	}

	/// <summary> Gets the current default currency exchange rate. </summary>
	/// <param name="cancellationToken"> Cancellation token. </param>
	/// <response code="200"> The default currency exchange rate was successfully obtained. </response>
	/// <response code="403"> You do not have permission to access this endpoint. </response>
	/// <response code="404"> The requested endpoint could not be found. </response>
	/// <response code="422"> A validation error occurred while processing the request. </response>
	/// <response code="429"> You have reached your rate or monthly limit for accessing this endpoint. </response>
	/// <response code="500"> An internal server error occurred while processing the request. </response>
	[HttpGet(""), ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrentCurrency(CancellationToken cancellationToken)
	{
		CurrencyDto currencyDto = await _internalService.GetCurrentCurrencyAsync(cancellationToken);

		return Ok(currencyDto.Adapt<CurrencyResponse>());
	}

	/// <summary> Gets the current default currency exchange rate for the specified date. </summary>
	/// <param name="date"> The date on which you want to get default currency exchange rate. </param>
	/// <param name="cancellationToken"> Cancellation token. </param>
	/// <response code="200"> The default currency exchange rate was successfully obtained. </response>
	/// <response code="403"> You do not have permission to access this endpoint. </response>
	/// <response code="404"> The requested endpoint could not be found. </response>
	/// <response code="422"> A validation error occurred while processing the request. </response>
	/// <response code="429"> You have reached your rate or monthly limit for accessing this endpoint. </response>
	/// <response code="500"> An internal server error occurred while processing the request. </response>
	[HttpGet("{date}"), ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrencyOnDate([FromRoute] DateOnly date, CancellationToken cancellationToken)
	{
		CurrencyDto currencyDto = await _internalService.GetCurrencyOnDateAsync(date, cancellationToken);

		return Ok(currencyDto.Adapt<CurrencyResponse>());
	}

	/// <summary> Gets the current favorite currencies exchange rate. </summary>
	/// <param name="name"> The name of favorite currencies you want to get. </param>
	/// <param name="cancellationToken"> Cancellation token. </param>
	/// <response code="200"> The favorite currencies exchange rate was successfully obtained. </response>
	/// <response code="403"> You do not have permission to access this endpoint. </response>
	/// <response code="404"> The requested endpoint could not be found. </response>
	/// <response code="422"> A validation error occurred while processing the request. </response>
	/// <response code="429"> You have reached your rate or monthly limit for accessing this endpoint. </response>
	/// <response code="500"> An internal server error occurred while processing the request. </response>
	[HttpGet("favorites/{name}"), ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrentFavoritesByName([FromRoute] string name, CancellationToken cancellationToken)
	{
		CurrencyDto currencyDto = await _internalService.GetCurrentFavoritesByNameAsync(name, cancellationToken);

		return Ok(currencyDto.Adapt<CurrencyResponse>());
	}

	/// <summary> Gets the current favorite currencies exchange rate for the specified date. </summary>
	/// <param name="name"> The name of favorite currencies you want to get. </param>
	/// <param name="date"> The date on which you want to get favorite currencies exchange rate. </param>
	/// <param name="cancellationToken"> Cancellation token. </param>
	/// <response code="200"> The favorite currencies exchange rate was successfully obtained. </response>
	/// <response code="403"> You do not have permission to access this endpoint. </response>
	/// <response code="404"> The requested endpoint could not be found. </response>
	/// <response code="422"> A validation error occurred while processing the request. </response>
	/// <response code="429"> You have reached your rate or monthly limit for accessing this endpoint. </response>
	/// <response code="500"> An internal server error occurred while processing the request. </response>
	[HttpGet("favorites/{name}/{date}"), ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetFavoritesOnDate([FromRoute] string name, [FromRoute] DateOnly date, CancellationToken cancellationToken)
	{
		CurrencyDto currencyDto = await _internalService.GetFavoritesOnDateByNameAsync(name, date, cancellationToken);

		return Ok(currencyDto.Adapt<CurrencyResponse>());
	}

	/// <summary> Retrieves the API configuration settings. </summary>
	/// <param name="cancellationToken"> Cancellation token. </param>
	/// <response code="200"> The settings was successfully obtained. </response>
	/// <response code="403"> You do not have permission to access this endpoint. </response>
	/// <response code="404"> The requested endpoint could not be found. </response>
	/// <response code="500"> An internal server error occurred while processing the request. </response>
	[HttpGet("settings"), ProducesDefaultResponseType(typeof(SettingsResponse))]
	public async Task<IActionResult> GetSettings(CancellationToken cancellationToken)
	{
		FullSettingsDto settingsDto = await _internalService.GetSettingsAsync(cancellationToken);

		return Ok(settingsDto.Adapt<SettingsResponse>());
	}
}
