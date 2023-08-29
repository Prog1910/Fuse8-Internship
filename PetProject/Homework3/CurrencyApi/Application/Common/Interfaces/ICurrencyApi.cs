using Domain.Aggregates.CurrenciesOnDateAggregate;
using Domain.Aggregates.CurrencyAggregate;
using Domain.Aggregates.SettingsAggregate;

namespace Application.Common.Interfaces;

public interface ICurrencyApi
{
	Task<Currency[]> GetAllCurrentCurrenciesAsync(string baseCurrency, CancellationToken cancellationToken);

	Task<CurrenciesOnDate> GetAllCurrenciesOnDateAsync(string baseCurrency, DateOnly date, CancellationToken cancellationToken);

	Task<Settings> GetSettingsAsync(CancellationToken cancellationToken);
}