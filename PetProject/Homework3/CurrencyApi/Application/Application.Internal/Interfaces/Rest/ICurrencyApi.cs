using Domain.Aggregates;

namespace Application.Internal.Interfaces.Rest;

public interface ICurrencyApi
{
	Task<Currency[]> GetAllCurrentCurrenciesAsync(string baseCurrency, CancellationToken cancellationToken);

	Task<CurrenciesOnDate> GetAllCurrenciesOnDateAsync(string baseCurrency, DateOnly date, CancellationToken cancellationToken);

	Task<Settings> GetSettingsAsync(CancellationToken cancellationToken);
}