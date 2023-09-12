using Shared.Application.Dtos;

namespace PublicApi.Application.Interfaces.Rest;

public interface IFavoritesService
{
	Task<FavoritesDto?> GetFavoritesByNameAsync(string name, CancellationToken cancellationToken = default);

	Task<List<FavoritesDto>> GetAllFavoritesAsync(CancellationToken cancellationToken = default);

	Task UpdateFavoritesByNameAsync(FavoritesDto favoritesDto, string name, CancellationToken cancellationToken = default);

	Task AddFavoritesAsync(FavoritesDto favoritesDto, CancellationToken cancellationToken = default);

	Task DeleteFavoritesByNameAsync(string name, CancellationToken cancellationToken = default);
}
