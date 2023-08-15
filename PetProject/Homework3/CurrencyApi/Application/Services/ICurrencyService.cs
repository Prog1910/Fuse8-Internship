using Application.Services.Common;

namespace Application.Services;

public interface ICurrencyService
{
	Task<Currency> GetCurrencyByDefault();

	Task<Currency> GetCurrencyByCode(string currencyCode);

	Task<CurrencyOnDate> GetHistoricalCurrencyExchangeRate(string currencyCode, string date);

	Task<ApiSettings> GetApiSettings();
}