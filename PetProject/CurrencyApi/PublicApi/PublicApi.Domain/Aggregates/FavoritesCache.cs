using System.ComponentModel.DataAnnotations;

namespace PublicApi.Domain.Aggregates;

public sealed record FavoritesCache
{
	public int Id { get; set; }
	[StringLength(64, MinimumLength = 1)] public string Name { get; set; } = string.Empty;
	[StringLength(5, MinimumLength = 3)] public string CurrencyCode { get; set; } = string.Empty;
	[StringLength(5, MinimumLength = 3)] public string BaseCurrencyCode { get; set; } = string.Empty;
}
