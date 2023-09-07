using Application.Shared.Dtos;

namespace Application.Public.Interfaces.Rest;

public interface IFavoritesService
{
	Task<FavoritesDto?> GetFavoritesByNameAsync(string name, CancellationToken cancellationToken);

	Task<List<FavoritesDto>> GetAllFavoritesAsync();

	Task UpdateFavoritesByNameAsync(FavoritesDto favoritesDto, string name, CancellationToken cancellationToken);

	Task AddFavoritesAsync(FavoritesDto favoritesDto, CancellationToken cancellationToken);

	Task DeleteFavoritesByNameAsync(string name, CancellationToken cancellationToken);
}