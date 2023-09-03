using System.ComponentModel.DataAnnotations;

namespace Domain.Aggregates;

public sealed record SettingsCache
{
	public int Id { get; set; }
	[StringLength(maximumLength: 8, MinimumLength = 3)] public string DefaultCurrencyCode { get; set; } = string.Empty;
	[Range(minimum: 0, maximum: 10)] public int CurrencyRoundCount { get; set; }
}