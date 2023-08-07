using Fuse8_ByteMinds.SummerSchool.Domain.Models.Currency;
using Fuse8_ByteMinds.SummerSchool.Domain.Models.Status;

namespace Fuse8_ByteMinds.SummerSchool.Application.Contracts;

public interface ICurrencyService
{
	Task<Currency> GetDefaultExchangeRate();

	Task<Currency> GetExchangeRateByCode(string currencyCode);

	Task<CurrencyOnDate> GetHistoricalExchangeRateByCode(string currencyCode, string date);

	Task<Status> GetStatus();
}