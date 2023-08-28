using Domain.Aggregates.CurrencyAggregate;

namespace Domain.Aggregates.CurrenciesOnDateAggregate;

public sealed class CachedCurrencies
{
	public DateTime LastUpdatedAt { get; set; }
	public string BaseCurrency { get; set; } = string.Empty;
	public List<Currency> Currencies { get; set; } = new();
}