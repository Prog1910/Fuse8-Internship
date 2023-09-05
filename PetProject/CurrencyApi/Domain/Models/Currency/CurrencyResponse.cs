namespace Fuse8_ByteMinds.SummerSchool.Domain.Models.Currency;

/// <summary>
/// Currency response structure
/// </summary>
/// <param name="Meta">Holds useful information</param>
/// <param name="Data">Holds the actual currency information</param>
public record CurrencyResponse(
	CurrencyMeta Meta,
	Dictionary<string, Currency> Data);