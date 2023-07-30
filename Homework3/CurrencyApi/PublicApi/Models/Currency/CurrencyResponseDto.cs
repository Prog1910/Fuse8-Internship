using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Currency;

/// <summary>
/// Currency response structure
/// </summary>
/// <param name="Meta">Holds useful information</param>
/// <param name="Data">Holds the actual currency information</param>
public record CurrencyResponseDto(
	[property: JsonPropertyName("meta")] CurrencyMetaDto Meta,
	[property: JsonPropertyName("data")] Dictionary<string, CurrencyDataDto> Data
	);
