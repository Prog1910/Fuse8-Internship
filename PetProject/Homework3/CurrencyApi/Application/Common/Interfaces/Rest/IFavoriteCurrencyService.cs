using Application.Common.Services.Rest.Common.Dtos;

namespace Application.Common.Interfaces.Rest;

public interface IFavoriteCurrencyService
{
    Task<FavoriteCurrencyDto?> GetFavoriteCurrencyByNameAsync(string name);

    Task<List<FavoriteCurrencyDto>?> GetAllFavoriteCurrenciesAsync();

    Task UpdateFavoriteCurrencyByNameAsync(FavoriteCurrencyDto favoriteCurrencyDto);

    Task AddFavoriteCurrencyAsync(FavoriteCurrencyDto favoriteCurrencyDto);

    Task DeleteFavoriteCurrencyByNameAsync(string name);
}