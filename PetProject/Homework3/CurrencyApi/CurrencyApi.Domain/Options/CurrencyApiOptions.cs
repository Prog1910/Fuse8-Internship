namespace CurrencyApi.Domain.Options;

public sealed class CurrencyApiOptions
{
	public const string SectionName = "CurrencyApi";
	public string ApiKey { get; init; } = string.Empty;
	public string BaseUrl { get; init; } = string.Empty;
	public string BaseCurrency { get; init; } = string.Empty;
	public string DefaultCurrency { get; init; } = string.Empty;
	public int CurrencyRoundCount { get; set; }
}