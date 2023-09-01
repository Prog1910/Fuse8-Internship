using Application.Shared.Dtos;

namespace Application.Public.Interfaces.Rest;

public interface IFavoriteCurrencyService
{
	Task<FavoriteCurrencyDto?> GetFavoriteCurrencyByNameAsync(string name);

	Task<List<FavoriteCurrencyDto>?> GetAllFavoriteCurrenciesAsync();

	Task UpdateFavoriteCurrencyByNameAsync(FavoriteCurrencyDto favoriteCurrencyDto);

	Task AddFavoriteCurrencyAsync(FavoriteCurrencyDto favoriteCurrencyDto);

	Task DeleteFavoriteCurrencyByNameAsync(string name);
}