using System.ComponentModel.DataAnnotations;

namespace Domain.Aggregates;

public sealed class CachedCurrencies
{
    public DateTime LastUpdatedAt { get; set; }
    [StringLength(maximumLength: 8, MinimumLength = 3)] public string BaseCurrency { get; set; } = string.Empty;
    public List<Currency> Currencies { get; set; } = new();
}