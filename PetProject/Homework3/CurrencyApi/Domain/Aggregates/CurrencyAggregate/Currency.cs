using System.ComponentModel.DataAnnotations;

namespace Domain.Aggregates.CurrencyAggregate;

/// <summary>
///     Represents a currency with its code and value information.
/// </summary>
/// <param name="Code">The currency code.</param>
/// <param name="Value">The currency value.</param>
public sealed class Currency
{
	[StringLength(maximumLength: 8, MinimumLength = 3)] public string Code { get; set; } = string.Empty;

	[Range(0, (double)decimal.MaxValue)] public decimal Value { get; set; }
}