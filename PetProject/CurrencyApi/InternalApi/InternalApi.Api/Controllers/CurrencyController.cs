using InternalApi.Application.Interfaces.Background;
using InternalApi.Application.Interfaces.Rest;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Dtos;
using Shared.Contracts;
using Shared.Contracts.Enums;

namespace InternalApi.Api.Controllers;

/// <summary> Controller for handling currency-related operations. </summary>
[ApiController, Route("currency-api")]
public sealed class CurrencyController : ControllerBase
{
	private readonly ICacheCurrencyApi _cacheCurrencyService;
	private readonly ICacheTaskManagerService _cacheTaskManagerService;

	public CurrencyController(ICacheCurrencyApi cacheCurrencyService, ICacheTaskManagerService cacheTaskManagerService)
	{
		_cacheCurrencyService = cacheCurrencyService;
		_cacheTaskManagerService = cacheTaskManagerService;
	}

	/// <summary> Gets the current currency exchange rate. </summary>
	/// <param name="currencyCode"> The currency you want to get exchange rate. </param>
	/// <param name="cancellationToken"> Cancellation token. </param>
	/// <response code="200"> The currency exchange rate was successfully obtained. </response>
	/// <response code="403"> You do not have permission to access this endpoint. </response>
	/// <response code="404"> The requested endpoint could not be found. </response>
	/// <response code="422"> A validation error occurred while processing the request. </response>
	/// <response code="429"> You have reached your rate or monthly limit for accessing this endpoint. </response>
	/// <response code="500"> An internal server error occurred while processing the request. </response>
	[HttpGet("currencies"), ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrentCurrency([FromQuery] CurrencyType currencyCode, CancellationToken cancellationToken)
	{
		CurrencyDto currencyDto = await _cacheCurrencyService.GetCurrentCurrencyAsync((Shared.Domain.Enums.CurrencyType)currencyCode, cancellationToken);

		return Ok(currencyDto.Adapt<CurrencyResponse>());
	}

	/// <summary> Gets the currency exchange rate for the specified date. </summary>
	/// <param name="currencyCode"> The currency you want to get exchange rate. </param>
	/// <param name="date"> The date on which you want to get currency exchange rate. </param>
	/// <param name="cancellationToken"> </param>
	/// <response code="200"> The currency exchange rate was successfully obtained. </response>
	/// <response code="403"> You do not have permission to access this endpoint. </response>
	/// <response code="404"> The requested endpoint could not be found. </response>
	/// <response code="422"> A validation error occurred while processing the request. </response>
	/// <response code="429"> You have reached your rate or monthly limit for accessing this endpoint. </response>
	/// <response code="500"> An internal server error occurred while processing the request. </response>
	[HttpGet("currencies/{date}"), ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrencyOnDate([FromQuery] CurrencyType currencyCode, [FromRoute] DateOnly date, CancellationToken cancellationToken)
	{
		CurrencyDto currencyDto = await _cacheCurrencyService.GetCurrencyOnDateAsync((Shared.Domain.Enums.CurrencyType)currencyCode, date, cancellationToken);

		return Ok(currencyDto.Adapt<CurrencyResponse>());
	}

	/// <summary> Gets the current favorite currencies exchange rate. </summary>
	/// <param name="currencyCode"> The currency of favorite currencies you want to get exchange rate. </param>
	/// <param name="baseCurrencyCode"> The base currency of favorite currencies. </param>
	/// <param name="cancellationToken"> Cancellation token. </param>
	/// <response code="200"> The favorite currencies exchange rate was successfully obtained. </response>
	/// <response code="403"> You do not have permission to access this endpoint. </response>
	/// <response code="404"> The requested endpoint could not be found. </response>
	/// <response code="422"> A validation error occurred while processing the request. </response>
	/// <response code="429"> You have reached your rate or monthly limit for accessing this endpoint. </response>
	/// <response code="500"> An internal server error occurred while processing the request. </response>
	[HttpGet("favorites/"), ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrentFavoritesByName([FromQuery] CurrencyType currencyCode, [FromQuery] CurrencyType baseCurrencyCode,
		CancellationToken cancellationToken)
	{
		CurrencyDto currencyDto = await _cacheCurrencyService.GetCurrencyByFavoritesAsync((Shared.Domain.Enums.CurrencyType)currencyCode,
			(Shared.Domain.Enums.CurrencyType)baseCurrencyCode, default, cancellationToken);

		return Ok(currencyDto.Adapt<CurrencyResponse>());
	}

	/// <summary> Gets the favorite currencies exchange rate for the specified date. </summary>
	/// <param name="currencyCode"> The currency of favorite currencies you want to get exchange rate. </param>
	/// <param name="baseCurrencyCode"> The base currency of favorite currencies. </param>
	/// <param name="date"> The date on which you want to get default currency exchange rate. </param>
	/// <param name="cancellationToken"> Cancellation token. </param>
	/// <response code="200"> The favorite currencies exchange rate was successfully obtained. </response>
	/// <response code="403"> You do not have permission to access this endpoint. </response>
	/// <response code="404"> The requested endpoint could not be found. </response>
	/// <response code="422"> A validation error occurred while processing the request. </response>
	/// <response code="429"> You have reached your rate or monthly limit for accessing this endpoint. </response>
	/// <response code="500"> An internal server error occurred while processing the request. </response>
	[HttpGet("favorites/{date}"), ProducesDefaultResponseType(typeof(CurrencyResponse))]
	public async Task<IActionResult> GetCurrentFavorites([FromQuery] CurrencyType currencyCode, [FromQuery] CurrencyType baseCurrencyCode,
		[FromRoute] DateOnly date, CancellationToken cancellationToken)
	{
		CurrencyDto currencyDto = await _cacheCurrencyService.GetCurrencyByFavoritesAsync((Shared.Domain.Enums.CurrencyType)currencyCode,
			(Shared.Domain.Enums.CurrencyType)baseCurrencyCode, date, cancellationToken);

		return Ok(currencyDto.Adapt<CurrencyResponse>());
	}

	/// <summary> Recalculates cached currency exchange rates relative to the specified base currency. </summary>
	/// <param name="baseCurrencyCode"> The base currency for which exchange rates should be recalculated. </param>
	/// <param name="cancellationToken"> Cancellation token. </param>
	/// <response code="200"> Currencies was successfully recalculated. </response>
	/// <response code="404"> The requested endpoint could not be found. </response>
	/// <response code="500"> An internal server error occurred while processing the request. </response>
	[HttpPost("/recalculation"), ProducesDefaultResponseType(typeof(Guid))]
	public async Task<AcceptedResult> RecalculateCache([FromQuery] CurrencyType baseCurrencyCode, CancellationToken cancellationToken)
	{
		Guid taskId = await _cacheTaskManagerService.RecalculateCacheAsync((Shared.Domain.Enums.CurrencyType)baseCurrencyCode, cancellationToken);

		return Accepted(taskId);
	}
}
