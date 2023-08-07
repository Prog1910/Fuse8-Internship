using Fuse8_ByteMinds.SummerSchool.Domain.Models.Currency;
using Fuse8_ByteMinds.SummerSchool.Domain.Models.Status;

namespace Fuse8_ByteMinds.SummerSchool.Application.Services;

public interface ICurrencyService
{
    Task<CurrencyData> GetDefaultExchangeRate();
    Task<CurrencyData> GetExchangeRateByCode(string currencyCode);
	Task<HistoricalCurrencyData> GetHistoricalExchangeRateByCode(string currencyCode, string date);
    Task<StatusData> GetStatus();
}
