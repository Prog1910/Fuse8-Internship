using System.ComponentModel.DataAnnotations;

namespace Domain.Aggregates;

public sealed class CachedSettings
{
	public int Id { get; set; }
	[StringLength(maximumLength: 8, MinimumLength = 3)] public string DefaultCurrency { get; set; } = string.Empty;
	[Range(minimum: 0, 10)] public int CurrencyRoundCount { get; set; }
}