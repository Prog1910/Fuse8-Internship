using Shared.Application.Dtos;

namespace PublicApi.Application.Interfaces.Rest;

public interface IInternalApi
{
	public Task<CurrencyDto> GetCurrentCurrencyAsync(CancellationToken cancellationToken = default);

	public Task<CurrencyDto> GetCurrencyOnDateAsync(DateOnly date, CancellationToken cancellationToken = default);

	public Task<CurrencyDto> GetCurrentFavoritesByNameAsync(string name, CancellationToken cancellationToken = default);

	public Task<CurrencyDto> GetFavoritesOnDateByNameAsync(string name, DateOnly date, CancellationToken cancellationToken = default);

	public Task<FullSettingsDto> GetSettingsAsync(CancellationToken cancellationToken = default);
}
