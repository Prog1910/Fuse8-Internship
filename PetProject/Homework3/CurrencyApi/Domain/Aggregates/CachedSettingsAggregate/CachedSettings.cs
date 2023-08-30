using System.ComponentModel.DataAnnotations;

namespace Domain.Aggregates.CachedSettingsAggregate;

public sealed class CachedSettings
{
	public int Id { get; set; }
	[StringLength(maximumLength: 8, MinimumLength = 3)] public string DefaultCurrency { get; set; } = string.Empty;
	[Range(0, int.MaxValue)] public int CurrencyRoundCount { get; set; }
}