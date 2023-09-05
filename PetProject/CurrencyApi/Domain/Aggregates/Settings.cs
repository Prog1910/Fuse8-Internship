namespace Domain.Aggregates;

/// <summary>
///     Represents application settings.
/// </summary>
/// <param name="BaseCurrencyCode">The base currency.</param>
/// <param name="NewRequestsAvailable">Indicates if new requests are available.</param>
public sealed record Settings
{
	public string BaseCurrencyCode { get; set; } = string.Empty;
	public bool NewRequestsAvailable { get; set; }
}