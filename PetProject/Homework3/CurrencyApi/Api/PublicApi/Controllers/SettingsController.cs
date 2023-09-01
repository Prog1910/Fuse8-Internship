using Application.Public.Interfaces.Rest;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Controllers;

/// <summary>
///     Controller for managing application settings.
/// </summary>
[ApiController]
[Route("currency-api/settings")]
public sealed class SettingsController : ControllerBase
{
	private readonly ISettingsWriterService _settingsService;

	public SettingsController(ISettingsWriterService settingsService)
	{
		_settingsService = settingsService;
	}

	/// <summary>
	///     Updates the default currency.
	/// </summary>
	/// <response code="200">The default currency was successfully updated.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpPut("default-currency")]
	[ProducesDefaultResponseType(typeof(void))]
	public async Task<IActionResult> UpdateDefaultCurrency([FromQuery] Contracts.Enums.CurrencyType defaultCurrency)
	{
		await _settingsService.UpdateDefaultCurrency((Domain.Enums.CurrencyType)defaultCurrency);

		return Accepted();
	}

	/// <summary>
	///     Updates the currency round count.
	/// </summary>
	/// <response code="200">The currency round count was successfully updated.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpPut("currency-round-count")]
	[ProducesDefaultResponseType(typeof(void))]
	public async Task<IActionResult> UpdateCurrencyRoundCount([FromQuery] int currencyRoundCount)
	{
		await _settingsService.UpdateCurrencyRoundCount(currencyRoundCount);

		return Accepted();
	}
}