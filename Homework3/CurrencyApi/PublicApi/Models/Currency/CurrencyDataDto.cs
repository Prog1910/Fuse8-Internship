using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Currency;

public record CurrencyDataDto(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("value")] decimal Value
    );
