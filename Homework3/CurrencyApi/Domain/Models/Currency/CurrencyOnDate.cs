namespace Fuse8_ByteMinds.SummerSchool.Domain.Models.Currency;

/// <summary>
/// Exchange rates for a specific date
/// </summary>
/// <param name="Date">Date of data update</param>
/// <param name="Code">Currency code</param>
/// <param name="Value">Currency rate</param>
public record CurrencyOnDate(
	string Date,
	string Code,
	decimal Value) : Currency(Code, Value);