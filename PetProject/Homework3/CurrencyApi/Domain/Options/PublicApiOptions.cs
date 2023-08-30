namespace Domain.Options;

public sealed class PublicApiOptions
{
	public const string SectionName = "CurrencyApi";
	public string BaseCurrency { get; set; } = string.Empty;
}