using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Currency;

/// <summary>
/// Currency response structure
/// </summary>
/// <param name="CurrencyMeta">Holds useful information</param>
/// <param name="CurrenciesData">Holds the actual currency information</param>
public record CurrencyResponseDto(
	[property: JsonPropertyName("meta")] CurrencyMetaDto CurrencyMeta,
	[property: JsonPropertyName("data")] Dictionary<string, CurrencyDataDto> CurrenciesData
	);
