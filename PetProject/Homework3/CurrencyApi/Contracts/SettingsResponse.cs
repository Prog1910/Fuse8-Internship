namespace Contracts;

/// <summary>
///     Represents a response containing settings information.
/// </summary>
/// <param name="DefaultCurrencyCode">The default currency.</param>
/// <param name="BaseCurrencyCode">The base currency.</param>
/// <param name="NewRequestsAvailable">Indicates if new requests are available.</param>
/// <param name="CurrencyRoundCount">The currency round count.</param>
public sealed record SettingsResponse(
	string DefaultCurrencyCode,
	string BaseCurrencyCode,
	bool NewRequestsAvailable,
	int CurrencyRoundCount);