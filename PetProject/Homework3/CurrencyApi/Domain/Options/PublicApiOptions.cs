namespace Domain.Options;

public sealed class PublicApiOptions
{
	public const string SectionName = "CurrencyApi";
	public string DefaultCurrency { get; init; } = string.Empty;
	public int CurrencyRoundCount { get; set; }
}
