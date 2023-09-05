namespace Domain.Aggregates;

/// <summary>
///     Represents currencies information at a specific date.
/// </summary>
/// <param name="LastUpdatedAt">The date and time when the currencies were last updated.</param>
/// <param name="Currencies">An array of currencies.</param>
public sealed record CurrenciesOnDate
{
	public DateTime LastUpdatedAt { get; set; }
	public Currency[] Currencies { get; set; } = null!;
}