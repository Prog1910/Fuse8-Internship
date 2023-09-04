using Application.Public.Persistence;
using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Public.Persistence.Repositories;

public sealed class FavoritesRepository : IFavoritesRepository
{
	private readonly UserDbContext _userDbContext;
	private readonly DbSet<FavoritesCache> _favoritesDbSet;

	public FavoritesRepository(UserDbContext userDbContext)
	{
		_userDbContext = userDbContext;
		_favoritesDbSet = _userDbContext.Favorites ?? throw new Exception("Favorites not found.");
	}

	public bool TryAddFavorites(FavoritesCache favorites)
	{
		if (IsUnique(favorites) is false) return false;

		_favoritesDbSet.Add(favorites);
		_userDbContext.SaveChanges();

		return true;
	}

	public FavoritesCache? GetFavoritesByName(string name)
	{
		var favorites = _favoritesDbSet.SingleOrDefault(f => f.Name.Equals(name));

		return favorites;
	}

	public IEnumerable<FavoritesCache> GetAllFavorites() => _favoritesDbSet.ToList() ?? throw new Exception("Favorites not found in cache.");

	public bool TryUpdateFavoritesByName(FavoritesCache favorites, string name)
	{
		if (GetFavoritesByName(name) is not { } favoritesToUpdate) return false;

		favoritesToUpdate.Name = favorites.Name;
		favoritesToUpdate.CurrencyCode = favorites.CurrencyCode;
		favoritesToUpdate.BaseCurrencyCode = favorites.BaseCurrencyCode;

		_favoritesDbSet.Update(favorites);
		_userDbContext.SaveChanges();
		return true;
	}

	public bool TryDeleteFavoritesByName(string name)
	{
		if (GetFavoritesByName(name) is not { } favorites) return false;

		_favoritesDbSet.Remove(favorites);
		_userDbContext.SaveChanges();

		return true;
	}

	private bool IsUnique(FavoritesCache favorites)
	{
		var isUniqueByName = IsUniqueByName(favorites);
		var isCurrencyCodesUnique = IsUniqueByCode(favorites);
		return isUniqueByName && isCurrencyCodesUnique;
	}

	private bool IsUniqueByCode(FavoritesCache favorites)
		=> GetAllFavorites().Any(f => f.CurrencyCode.Equals(favorites.CurrencyCode) && f.BaseCurrencyCode.Equals(favorites.BaseCurrencyCode)) is false;

	private bool IsUniqueByName(FavoritesCache favorites) => GetFavoritesByName(favorites.Name) is null;
}