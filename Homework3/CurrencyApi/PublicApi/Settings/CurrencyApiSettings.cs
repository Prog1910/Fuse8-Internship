namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Settings;

public class CurrencyApiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseCurrency { get; set; } = string.Empty;
    public string DefaultCurrency { get; set; } = string.Empty;
    public int CurrencyRoundCount { get; set; }
}