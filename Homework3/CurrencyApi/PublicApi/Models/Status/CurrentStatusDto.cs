using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Status;

/// <summary>
/// Current API settings
/// </summary>
/// <param name="DefaultCurrency">Current default exchange rate from configuration</param>
/// <param name="BaseCurrency">Base currency against which the exchange rate is calculated</param>
/// <param name="RequestLimit">Total number of available requests received from external API</param>
/// <param name="RequestCount">Number of used requests received from external API</param>
/// <param name="CurrencyRoundCount">The number of decimal places to which the value of the exchange rate should be rounded</param>
public record CurrentStatusDto(
    [property: JsonPropertyName("default_currency")] string DefaultCurrency,
    [property: JsonPropertyName("base_currency")] string BaseCurrency,
    [property: JsonPropertyName("request_limit")] int RequestLimit,
    [property: JsonPropertyName("request_count")] int RequestCount,
    [property: JsonPropertyName("currency_round_count")] int CurrencyRoundCount
    );
