namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Settings;

public class CurrencyApiSettings
{
    public string ApiKey { get; init; } = string.Empty;
    public string BaseUrl { get; init; } = string.Empty;
    public string BaseCurrency { get; init; } = string.Empty;
    public string DefaultCurrency { get; init; } = string.Empty;
    public int CurrencyRoundCount { get; set; }
}