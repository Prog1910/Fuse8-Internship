using CurrencyApi.Domain.Aggregates.CurrenciesOnDateAggregate;
using CurrencyApi.Domain.Aggregates.CurrencyAggregate;
using CurrencyApi.Domain.Aggregates.SettingsAggregate;

namespace CurrencyApi.Application.Common.Services.Interfaces;

public interface ICurrencyApi
{
	Task<Currency[]> GetAllCurrentCurrenciesAsync(string baseCurrency, CancellationToken cancellationToken);

	Task<CurrenciesOnDate> GetAllCurrenciesOnDateAsync(string baseCurrency, DateOnly date, CancellationToken cancellationToken);

	Task<Settings> GetSettingsAsync(CancellationToken cancellationToken);
}