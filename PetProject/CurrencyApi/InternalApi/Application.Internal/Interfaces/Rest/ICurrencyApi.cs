using Domain.Aggregates;

namespace Application.Internal.Interfaces.Rest;

public interface ICurrencyApi
{
	Task<Currency[]> GetAllCurrentCurrenciesAsync(string baseCurrencyCode, CancellationToken cancellationToken = default);

	Task<CurrenciesOnDate> GetAllCurrenciesOnDateAsync(string baseCurrencyCode, DateOnly date, CancellationToken cancellationToken = default);

	Task<Settings> GetSettingsAsync(CancellationToken cancellationToken = default);
}
