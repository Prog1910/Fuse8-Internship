using System.ComponentModel.DataAnnotations;

namespace Domain.Aggregates;

public sealed record FavoritesCache
{
	[StringLength(maximumLength: 64, MinimumLength = 1)] public string Name { get; set; } = string.Empty;
	[StringLength(maximumLength: 5, MinimumLength = 3)] public string CurrencyCode { get; set; } = string.Empty;
	[StringLength(maximumLength: 5, MinimumLength = 3)] public string BaseCurrencyCode { get; set; } = string.Empty;
}