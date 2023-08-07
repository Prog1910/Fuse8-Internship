namespace Fuse8_ByteMinds.SummerSchool.Domain.Models.Currency;

/// <summary>
/// Currency exchange rate
/// </summary>
/// <param name="Code">Currency code</param>
/// <param name="Value">The value of the exchange rate, relative to the base currency</param>
public record Currency(
	string Code,
	decimal Value);