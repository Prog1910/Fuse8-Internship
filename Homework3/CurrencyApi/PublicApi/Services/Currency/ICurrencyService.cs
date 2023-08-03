using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Currency;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Status;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services.Currency;

public interface ICurrencyService
{
    Task<ExchangeRate> GetCurrencyExchangeRate();
    Task<ExchangeRate> GetCurrencyExchangeRateByCode(string currencyCode);
    Task<HistoricalExchangeRate> GetHistoricalCurrencyExchangeRate(string currencyCode, string date);
    Task<Status> GetApiSettings();
}