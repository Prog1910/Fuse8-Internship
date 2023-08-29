using Application.Common.Services.Common.Dtos;
using Domain.Enums;

namespace Application.Common.Interfaces;

public interface ICachedCurrencyApi
{
	Task<CurrencyDto> GetCurrentCurrencyAsync(CurrencyType defaultCurrency, CancellationToken cancellationToken);

	Task<CurrencyDto> GetCurrencyOnDateAsync(CurrencyType defaultCurrency, DateOnly date, CancellationToken cancellationToken);

	Task<CurrencyDto> GetCurrentFavoriteCurrencyAsync(CurrencyType defaultCurrency, CurrencyType baseCurrency, CancellationToken cancellationToken);

	Task<SettingsDto> GetSettingsAsync(CancellationToken cancellationToken);
}
