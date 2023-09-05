using Domain.Aggregates;

namespace Application.Public.Persistence;

public interface IFavoritesRepository
{
	bool TryAddFavorites(FavoritesCache favorites);

	FavoritesCache? GetFavoritesByName(string name);

	IEnumerable<FavoritesCache>? GetAllFavorites();

	bool TryUpdateFavoritesByName(FavoritesCache favorites, string name);

	bool TryDeleteFavoritesByName(string name);
}