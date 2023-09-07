namespace Domain.Options;

public sealed class PublicApiOptions
{
	public const string SectionName = "CurrencyApi";
	public string BaseCurrencyCode { get; set; } = string.Empty;
}
