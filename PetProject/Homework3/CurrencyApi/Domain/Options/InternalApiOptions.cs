namespace Domain.Options;

public sealed class InternalApiOptions
{
	public const string SectionName = "CurrencyApi";
	public string ApiKey { get; init; } = string.Empty;
	public string BaseUrl { get; init; } = string.Empty;
	public string BaseCurrency { get; init; } = string.Empty;
}
