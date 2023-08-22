namespace CurrencyApi.Domain.Aggregates.SettingsAggregate;

/// <summary>
/// Represents application settings.
/// </summary>
/// <param name="DefaultCurrency">The default currency.</param>
/// <param name="BaseCurrency">The base currency.</param>
/// <param name="NewRequestsAvailable">Indicates if new requests are available.</param>
/// <param name="CurrencyRoundCount">The currency round count.</param>
public sealed record Settings(
string DefaultCurrency,
string BaseCurrency,
bool NewRequestsAvailable,
int CurrencyRoundCount);