using Application.Shared.Dtos;

namespace Application.Public.Interfaces.Rest;

public interface IInternalApi
{
	public Task<CurrencyDto> GetCurrentCurrencyAsync(CancellationToken cancellationToken);

	public Task<CurrencyDto> GetCurrencyOnDateAsync(DateOnly date, CancellationToken cancellationToken);

	public Task<CurrencyDto> GetCurrentFavoritesByNameAsync(string name, CancellationToken cancellationToken);

	public Task<CurrencyDto> GetFavoritesOnDateByNameAsync(string name, DateOnly date, CancellationToken cancellationToken);

	public Task<FullSettingsDto> GetSettingsAsync(CancellationToken cancellationToken);
}
