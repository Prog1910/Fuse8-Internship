using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Currency;

public record CurrencyResponseDto(
	[property: JsonPropertyName("meta")] CurrencyMetaDto CurrencyMeta,
	[property: JsonPropertyName("data")] Dictionary<string, CurrencyDataDto> CurrenciesData
	);
