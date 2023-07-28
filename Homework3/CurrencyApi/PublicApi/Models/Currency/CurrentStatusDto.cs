using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Currency;

public record CurrentStatusDto(
    [property: JsonPropertyName("default_currency")] string DefaultCurrency,
    [property: JsonPropertyName("base_currency")] string BaseCurrency,
    [property: JsonPropertyName("request_limit")] int RequestLimit,
    [property: JsonPropertyName("request_count")] int RequestCount,
    [property: JsonPropertyName("currency_round_count")] int CurrencyRoundCount
    );
