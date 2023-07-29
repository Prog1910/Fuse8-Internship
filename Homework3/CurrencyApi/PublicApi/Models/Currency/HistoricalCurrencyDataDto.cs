using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Currency;

/// <summary>
/// Historical currency information
/// </summary>
/// <param name="Date">Date of exchange rate relevance</param>
/// <param name="Code">Currency code</param>
/// <param name="Value">Currency rate</param>
public record HistoricalCurrencyDataDto(
	[property: JsonPropertyName("date")] string Date,
	string Code,
	decimal Value
	) : CurrencyDataDto(Code, Value);
