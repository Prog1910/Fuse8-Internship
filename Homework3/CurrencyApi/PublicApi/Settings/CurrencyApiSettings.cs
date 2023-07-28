namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Settings;

public class CurrencyApiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseCurrency { get; set; } = string.Empty;
    public string TargetCurrency { get; set; } = string.Empty;
    public int DecimalPlaces { get; set; }
}