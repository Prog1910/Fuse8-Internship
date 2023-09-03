using Application.Public.Persistence;
using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Public.Persistence.Repositories;

public sealed class FavoritesRepository : IFavoritesRepository
{
	private readonly UserDbContext _context;
	private readonly DbSet<FavoritesCache> _favorites;

	public FavoritesRepository(UserDbContext context)
	{
		_context = context;
		_favorites = _context.Favorites ?? throw new Exception("Favorites not found.");
	}

	public bool TryAddFavorites(FavoritesCache favorites)
	{
		if (GetFavoritesByName(favorites.Name) is not null) return false;

		_favorites.Add(favorites);
		_context.SaveChanges();

		return true;
	}

	public FavoritesCache? GetFavoritesByName(string name)
	{
		var favorites = _favorites.SingleOrDefault(f => f.Name.Equals(name));

		return favorites;
	}

	public IEnumerable<FavoritesCache> GetAllFavorites() => _favorites.AsEnumerable();

	public bool TryUpdateFavoritesByName(FavoritesCache favorites, string name)
	{
		if (GetFavoritesByName(name) is null) return false;

		_favorites.Update(favorites);

		return true;
	}

	public bool TryDeleteFavoritesByName(string name)
	{
		if (GetFavoritesByName(name) is not { } favorites) return true;

		_favorites.Remove(favorites);
		_context.SaveChanges();

		return true;
	}
}