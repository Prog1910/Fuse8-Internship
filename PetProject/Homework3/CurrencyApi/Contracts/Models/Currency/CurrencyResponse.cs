using System.Text.Json.Serialization;

namespace Contracts.Models.Currency;

/// <summary>
/// Currency response structure
/// </summary>
/// <param name="Meta">Holds useful information</param>
/// <param name="Data">Holds the actual currency information</param>
public record CurrencyResponse(
	[property: JsonPropertyName("meta")] CurrencyMetaResponse Meta,
	[property: JsonPropertyName("data")] Dictionary<string, CurrencyDataResponse> Data);

/// <summary>
/// Information about last updates
/// </summary>
/// <param name="LastUpdatedAt">Datetime to let you know then this dataset was last updated</param>
public record CurrencyMetaResponse(
	[property: JsonPropertyName("last_updated_at")] string LastUpdatedAt);

/// <summary>
/// Actual currency information
/// </summary>
/// <param name="Code">Currency code</param>
/// <param name="Value">Currency rate</param>
public record CurrencyDataResponse(
	[property: JsonPropertyName("code")] string Code,
	[property: JsonPropertyName("value")] decimal Value);