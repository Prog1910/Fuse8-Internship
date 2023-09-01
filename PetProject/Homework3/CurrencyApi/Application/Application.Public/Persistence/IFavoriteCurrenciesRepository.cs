using Domain.Aggregates;

namespace Application.Public.Persistence;

public interface IFavoriteCurrenciesRepository
{
	bool TryAddFavoriteCurrencies(CachedFavoriteCurrency favoriteCurrency);

	CachedFavoriteCurrency? GetFavoriteCurrencyByName(string name);

	List<CachedFavoriteCurrency>? GetAllFavoriteCurrencies();

	bool TryUpdateFavoriteCurrencyByName(CachedFavoriteCurrency favoriteCurrency);

	bool TryDeleteFavoriteCurrencyByName(string name);
}