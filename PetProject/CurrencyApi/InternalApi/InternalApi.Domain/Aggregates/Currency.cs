using System.ComponentModel.DataAnnotations;

namespace InternalApi.Domain.Aggregates;

/// <summary> Represents a currency with its code and value information. </summary>
/// <param name="Code"> The currency code. </param>
/// <param name="Value"> The currency value. </param>
public sealed record Currency
{
	[StringLength(5, MinimumLength = 3)] public string Code { get; set; } = string.Empty;
	[Range(0, double.MaxValue)] public decimal Value { get; set; }
}
