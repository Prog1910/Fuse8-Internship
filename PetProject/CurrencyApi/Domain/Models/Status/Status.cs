namespace Fuse8_ByteMinds.SummerSchool.Domain.Models.Status;

/// <summary>
/// Current API options
/// </summary>
/// <param name="DefaultCurrency">Current default exchange rate from configuration</param>
/// <param name="BaseCurrency">Base currency against which the exchange rate is calculated</param>
/// <param name="RequestLimit">Total number of available requests received from external API</param>
/// <param name="RequestCount">Number of used requests received from external API</param>
/// <param name="CurrencyRoundCount">The number of decimal places to which the value of the exchange rate should be rounded</param>
public record Status(
	string DefaultCurrency,
	string BaseCurrency,
	int RequestLimit,
	int RequestCount,
	int CurrencyRoundCount);