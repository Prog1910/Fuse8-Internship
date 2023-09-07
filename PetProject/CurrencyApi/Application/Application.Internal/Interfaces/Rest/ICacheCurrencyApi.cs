using Application.Shared.Dtos;
using Domain.Enums;

namespace Application.Internal.Interfaces.Rest;

public interface ICacheCurrencyApi
{
	Task<CurrencyDto> GetCurrentCurrencyAsync(CurrencyType defaultCurrencyCode, CancellationToken cancellationToken);

	Task<CurrencyDto> GetCurrencyOnDateAsync(CurrencyType defaultCurrencyCode, DateOnly date, CancellationToken cancellationToken);

	Task<CurrencyDto> GetCurrencyByFavoritesAsync(CurrencyType favoriteCurrencyCode, CurrencyType favoriteBaseCurrencyCode, DateOnly? date, CancellationToken cancellationToken);

	Task<SettingsDto> GetSettingsAsync(CancellationToken cancellationToken);
}
