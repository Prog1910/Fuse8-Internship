using Application.Shared.Dtos;

namespace Application.Public.Interfaces.Rest;

public interface IFavoritesService
{
	Task<FavoritesDto?> GetFavoritesByNameAsync(string name);

	Task<IEnumerable<FavoritesDto>?> GetAllFavoritesAsync();

	Task UpdateFavoritesByNameAsync(FavoritesDto favoritesDto, string name);

	Task AddFavoritesAsync(FavoritesDto favoritesDto);

	Task DeleteFavoritesByNameAsync(string name);
}