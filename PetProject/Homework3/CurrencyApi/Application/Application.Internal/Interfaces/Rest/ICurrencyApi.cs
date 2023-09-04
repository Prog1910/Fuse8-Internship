using Domain.Aggregates;

namespace Application.Internal.Interfaces.Rest;

public interface ICurrencyApi
{
	Task<Currency[]> GetAllCurrentCurrenciesAsync(string baseCurrencyCode, CancellationToken cancellationToken);

	Task<CurrenciesOnDate> GetAllCurrenciesOnDateAsync(string baseCurrencyCode, DateOnly date, CancellationToken cancellationToken);

	Task<Settings> GetSettingsAsync(CancellationToken cancellationToken);
}