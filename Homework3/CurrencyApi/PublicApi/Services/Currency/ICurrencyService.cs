using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Currency;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Status;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services.Currency;

public interface ICurrencyService
{
    Task<CurrencyDataDto> GetCurrencyExchangeRate();
    Task<CurrencyDataDto> GetCurrencyExchangeRateByCode(string currencyCode);
    Task<HistoricalCurrencyDataDto> GetHistoricalCurrencyExchangeRate(string currencyCode, string date);
    Task<CurrentStatusDto> GetApiSettings();
}