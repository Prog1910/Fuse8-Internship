using Application.Persistence;
using Domain.Aggregates.CachedFavoriteCurrenciesAggregate;

namespace Infrastructure.Persistence.Repositories;

public sealed class FavoriteCurrenciesRepository : IFavoriteCurrenciesRepository
{
	private readonly PublicDbContext _context;

	public FavoriteCurrenciesRepository(PublicDbContext context)
	{
		_context = context;
	}

	public bool TryAddFavoriteCurrencies(CachedFavoriteCurrency favoriteCurrency)
	{
		if (GetFavoriteCurrencyByName(favoriteCurrency.Name) is not null) return false;

		_context.Add(favoriteCurrency);
		_context.SaveChanges();

		return true;
	}

	public CachedFavoriteCurrency? GetFavoriteCurrencyByName(string name)
	{
		var favorite = _context.FavoriteCurrencies.SingleOrDefault(fc => fc.Name.Equals(name));

		return favorite;
	}

	public List<CachedFavoriteCurrency>? GetAllFavoriteCurrencies()
	{
		var favorites = _context.FavoriteCurrencies.ToList();

		return favorites;
	}

	public bool TryUpdateFavoriteCurrencyByName(CachedFavoriteCurrency favoriteCurrency)
	{
		if (GetFavoriteCurrencyByName(favoriteCurrency.Name) is null) return false;

		_context.FavoriteCurrencies.Update(favoriteCurrency);

		return true;
	}

	public bool TryDeleteFavoriteCurrencyByName(string name)
	{
		var favorite = GetFavoriteCurrencyByName(name);
		if (favorite is null) return false;

		_context.Remove(favorite);
		_context.SaveChanges();

		return true;
	}
}