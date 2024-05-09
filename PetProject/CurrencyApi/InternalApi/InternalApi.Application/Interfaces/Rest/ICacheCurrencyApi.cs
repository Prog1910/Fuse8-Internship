using Shared.Application.Dtos;
using Shared.Domain.Enums;

namespace InternalApi.Application.Interfaces.Rest;

public interface ICacheCurrencyApi
{
	Task<CurrencyDto> GetCurrentCurrencyAsync(CurrencyType defaultCurrencyCode, CancellationToken cancellationToken = default);

	Task<CurrencyDto> GetCurrencyOnDateAsync(CurrencyType defaultCurrencyCode, DateOnly date, CancellationToken cancellationToken = default);

	Task<CurrencyDto> GetCurrencyByFavoritesAsync(CurrencyType favoriteCurrencyCode, CurrencyType favoriteBaseCurrencyCode, DateOnly? date,
		CancellationToken cancellationToken = default);

	Task<SettingsDto> GetSettingsAsync(CancellationToken cancellationToken = default);
}
