namespace Domain.Aggregates.CurrencyAggregate;

/// <summary>
/// Represents a currency with its code and value information.
/// </summary>
/// <param name="Code">The currency code.</param>
/// <param name="Value">The currency value.</param>
public sealed class Currency
{
	public string Code { get; set; } = string.Empty;
	public decimal Value { get; set; }
}