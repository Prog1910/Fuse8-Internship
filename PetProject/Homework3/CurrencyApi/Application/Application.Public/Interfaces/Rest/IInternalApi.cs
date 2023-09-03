using Application.Shared.Dtos;

namespace Application.Public.Interfaces.Rest;

public interface IInternalApi
{
	public Task<CurrencyDto> GetCurrentCurrencyAsync();

	public Task<CurrencyDto> GetCurrencyOnDateAsync(DateOnly date);

	public Task<CurrencyDto> GetCurrentFavoritesByNameAsync(string name);

	public Task<CurrencyDto> GetFavoritesOnDateByNameAsync(string name, DateOnly date);

	public Task<FullSettingsDto> GetSettingsAsync();
}