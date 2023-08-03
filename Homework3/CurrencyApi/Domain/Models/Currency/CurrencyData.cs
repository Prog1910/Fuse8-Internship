using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.Domain.Models.Currency;

/// <summary>
/// Actual currency information
/// </summary>
/// <param name="Code">Currency code</param>
/// <param name="Value">Currency rate</param>
public record CurrencyData(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("value")] decimal Value);
