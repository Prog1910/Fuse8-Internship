using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace InternalApi.Domain.Aggregates;

public sealed record CurrenciesOnDateCache
{
	public DateTime LastUpdatedAt { get; set; }
	[StringLength(5, MinimumLength = 3)] public string BaseCurrencyCode { get; set; } = string.Empty;
	public List<Currency> Currencies { get; set; } = new();
}
