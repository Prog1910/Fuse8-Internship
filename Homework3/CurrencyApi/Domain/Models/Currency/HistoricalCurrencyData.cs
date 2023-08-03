using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.Domain.Models.Currency;

/// <summary>
/// Historical currency information
/// </summary>
/// <param name="Date">Date of exchange rate relevance</param>
/// <param name="Code">Currency code</param>
/// <param name="Value">Currency rate</param>
public record HistoricalCurrencyData(
    [property: JsonPropertyName("date")] string Date,
    string Code,
    decimal Value) : CurrencyData(Code, Value);
