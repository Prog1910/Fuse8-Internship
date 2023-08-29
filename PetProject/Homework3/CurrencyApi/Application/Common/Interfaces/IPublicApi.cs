using Domain.Aggregates.CachedFavoriteCurrenciesAggregate;
using Domain.Enums;

namespace Application.Common.Interfaces;

public interface IPublicApi
{
	Task UpdateDefaultCurrencyAsync(CurrencyType defaultCurrency);

	Task UpdateCurrencyRoundCountAsync(int currencyRoundCount);

	Task<CachedFavoriteCurrency?> GetFavoriteCurrenciesByNameAsync(string name);

	Task<List<CachedFavoriteCurrency>?> GetAllFavoriteCurrenciesAsync();

	Task UpdateFavoriteCurrenciesByNameAsync(string name, CachedFavoriteCurrency favoriteCurrencies);

	Task AddFavoriteCurrenciesAsync(CachedFavoriteCurrency favoriteCurrencies);

	Task DeleteFavoriteCurrenciesByNameAsync(string name);
}
