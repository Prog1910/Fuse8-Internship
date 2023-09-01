using System.ComponentModel.DataAnnotations;

namespace Domain.Aggregates;

public sealed class CachedFavoriteCurrency
{
    [StringLength(maximumLength: 64, MinimumLength = 1)] public string Name { get; set; } = string.Empty;
    [StringLength(maximumLength: 8, MinimumLength = 3)] public string Currency { get; set; } = string.Empty;
    [StringLength(maximumLength: 8, MinimumLength = 3)] public string BaseCurrency { get; set; } = string.Empty;
}