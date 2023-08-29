namespace Domain.Aggregates.CachedSettingsAggregate;

public sealed class CachedSettings
{
	public int Id { get; set; }
	public string DefaultCurrency { get; set; } = string.Empty;
	public int CurrencyRoundCount { get; set; }
}
