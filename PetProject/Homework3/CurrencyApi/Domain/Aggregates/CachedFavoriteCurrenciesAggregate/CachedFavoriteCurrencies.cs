namespace Domain.Aggregates.CachedFavoriteCurrenciesAggregate;

public sealed class CachedFavoriteCurrencies
{
	public string Name { get; set; } = string.Empty;
	public string Currency { get; set; } = string.Empty;
	public string BaseCurrency { get; set; } = string.Empty;
}
