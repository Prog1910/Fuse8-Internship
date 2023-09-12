using InternalApi.Domain.Aggregates;

namespace InternalApi.Application.Interfaces.Rest;

public interface ICurrencyApi
{
	Task<Currency[]> GetAllCurrentCurrenciesAsync(string baseCurrencyCode, CancellationToken cancellationToken = default);

	Task<CurrenciesOnDate> GetAllCurrenciesOnDateAsync(string baseCurrencyCode, DateOnly date, CancellationToken cancellationToken = default);

	Task<Settings> GetSettingsAsync(CancellationToken cancellationToken = default);
}
