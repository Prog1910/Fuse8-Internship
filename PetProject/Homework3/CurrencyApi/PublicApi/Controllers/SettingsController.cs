using Application.Common.Interfaces;
using Domain.Aggregates.CachedFavoriteCurrenciesAggregate;
using Domain.Enums;
using Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Controllers;

/// <summary>
/// Controller for handling currency-related operations.
/// </summary>
[ApiController]
[Route("currency-api-settings")]
public sealed class SettingsController : ControllerBase
{
	private readonly IPublicApi _publicService;

	public SettingsController(IPublicApi publicService)
	{
		_publicService = publicService;
	}

	/// <summary>
	/// Updates the default currency.
	/// </summary>
	/// <response code="200">The default currency was successfully updated.</response>
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
	/// Updates the currency round count.
	/// </summary>
	/// <response code="200">The currency round count was successfully updated.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpPut("/settings/currency-round-count")]
	[ProducesDefaultResponseType(typeof(void))]
	public async Task<IActionResult> UpdateCurrencyRoundCount([FromQuery] int currencyRoundCount)
	{
		await _publicService.UpdateCurrencyRoundCountAsync(currencyRoundCount);

		return Accepted();
	}

	/// <summary>
	/// Retrieves the list of all favorite currencies
	/// </summary>
	/// <response code="200">The list of all favorite currencies was successfully obtained.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("/favorite-currencies")]
	[ProducesDefaultResponseType(typeof(List<CachedFavoriteCurrency>))]
	public async Task<IActionResult> GetAllFavoriteCurrencies([FromQuery] int currencyRoundCount)
	{
		var favorites = await _publicService.GetAllFavoriteCurrenciesAsync();

		return Ok(favorites);
	}

	/// <summary>
	/// Retrieves the favorite currencies
	/// </summary>
	/// <response code="200">The favorite currencies was successfully obtained.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("/favorite-currencies/{name}")]
	[ProducesDefaultResponseType(typeof(CachedFavoriteCurrency))]
	public async Task<IActionResult> GetFavoriteCurrenciesByName(string name)
	{
		var favorite = await _publicService.GetFavoriteCurrenciesByNameAsync(name);

		return Ok(favorite);
	}

	/// <summary>
	/// Adds the favorite currencies
	/// </summary>
	/// <response code="200">The favorite currencies was successfully added.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpPost("/favorite-currencies")]
	[ProducesDefaultResponseType(typeof(void))]
	public async Task<IActionResult> AddFavoriteCurrencies([FromQuery] CachedFavoriteCurrency favoriteCurrencies)
	{
		await _publicService.AddFavoriteCurrenciesAsync(favoriteCurrencies);

		return Accepted();
	}

	/// <summary>
	/// Updates the favorite currencies by name
	/// </summary>
	/// <response code="200">The favorite currencies was successfully updated.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpPut("/favorite-currencies/{name}")]
	[ProducesDefaultResponseType(typeof(void))]
	public async Task<IActionResult> UpdateFavoriteCurrenciesByName(string name, [FromQuery] CachedFavoriteCurrency favoriteCurrencies)
	{
		await _publicService.UpdateFavoriteCurrenciesByNameAsync(name, favoriteCurrencies);

		return Accepted();
	}

	/// <summary>
	/// Delete the favorite currencies by name
	/// </summary>
	/// <response code="200">The favorite currencies was successfully deleted.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpDelete("/favorite-currencies/{name}")]
	[ProducesDefaultResponseType(typeof(void))]
	public async Task<IActionResult> DeleteFavoriteCurrenciesByName(string name)
	{
		await _publicService.DeleteFavoriteCurrenciesByNameAsync(name);

		return Ok();
	}
}
