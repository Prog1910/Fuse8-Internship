namespace Domain.Aggregates.SettingsAggregate;

/// <summary>
/// Represents application settings.
/// </summary>
/// <param name="BaseCurrency">The base currency.</param>
/// <param name="NewRequestsAvailable">Indicates if new requests are available.</param>
public sealed record Settings
{
	public string BaseCurrency { get; set; } = string.Empty;
	public bool NewRequestsAvailable { get; set; }
}