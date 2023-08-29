using Domain.Aggregates.CachedFavoriteCurrenciesAggregate;

namespace Application.Persistence;

public interface IFavoriteCurrenciesRepository
{
	bool TryAddFavoriteCurrencies(CachedFavoriteCurrency favoriteCurrencies);

	CachedFavoriteCurrency? GetFavoriteCurrencyByName(string name);

	List<CachedFavoriteCurrency>? GetAllFavoriteCurrencies();

	bool TryUpdateFavoriteCurrencyByName(string name, CachedFavoriteCurrency favoriteCurrencies);

	bool TryDeleteFavoriteCurrencyByName(string name);
}
