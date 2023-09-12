using Microsoft.AspNetCore.Mvc;
using PublicApi.Application.Interfaces.Rest;
using Shared.Contracts.Enums;

namespace PublicApi.Api.Controllers;

/// <summary> Controller for managing application settings. </summary>
[ApiController, Route("currency-api/settings")]
public sealed class SettingsController : ControllerBase
{
	private readonly ISettingsService _settingsService;

	public SettingsController(ISettingsService settingsService)
	{
		_settingsService = settingsService;
	}

	/// <summary> Updates the default currency. </summary>
	/// <param name="defaultCurrencyCode"> The new default currency code. </param>
	/// <param name="cancellationToken"> Cancellation token. </param>
	/// <response code="200"> The default currency was successfully updated. </response>
	/// <response code="404"> The requested endpoint could not be found. </response>
	/// <response code="500"> An internal server error occurred while processing the request. </response>
	[HttpPut("default-currency-code"), ProducesDefaultResponseType(typeof(void))]
	public async Task<IActionResult> UpdateDefaultCurrency([FromQuery] CurrencyType defaultCurrencyCode, CancellationToken cancellationToken)
	{
		await _settingsService.UpdateDefaultCurrencyCodeAsync((Shared.Domain.Enums.CurrencyType)defaultCurrencyCode, cancellationToken);

		return NoContent();
	}

	/// <summary> Updates the currency round count. </summary>
	/// <param name="currencyRoundCount"> The new currency round count. </param>
	/// <param name="cancellationToken"> Cancellation token. </param>
	/// <response code="200"> The currency round count was successfully updated. </response>
	/// <response code="404"> The requested endpoint could not be found. </response>
	/// <response code="500"> An internal server error occurred while processing the request. </response>
	[HttpPut("currency-round-count"), ProducesDefaultResponseType(typeof(void))]
	public async Task<IActionResult> UpdateCurrencyRoundCount([FromQuery] int currencyRoundCount, CancellationToken cancellationToken)
	{
		await _settingsService.UpdateCurrencyRoundCountAsync(currencyRoundCount, cancellationToken);

		return NoContent();
	}
}
