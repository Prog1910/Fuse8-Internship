using System.ComponentModel.DataAnnotations;

namespace PublicApi.Domain.Aggregates;

public sealed record SettingsCache
{
	public int Id { get; set; }
	[StringLength(5, MinimumLength = 3)] public string DefaultCurrencyCode { get; set; } = string.Empty;
	[Range(0, 15)] public int CurrencyRoundCount { get; set; }
}
