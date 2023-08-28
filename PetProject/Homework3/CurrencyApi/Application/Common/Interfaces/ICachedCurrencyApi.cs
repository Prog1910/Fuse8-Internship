using Application.Common.Services.Common.Dtos;
using Domain.Enums;

namespace Application.Common.Interfaces;

public interface ICachedCurrencyApi
{
	Task<CurrencyDto> GetCurrentCurrencyAsync(CurrencyType currencyType, CancellationToken cancellationToken);

	Task<CurrencyDto> GetCurrencyOnDateAsync(CurrencyType currencyType, DateOnly date, CancellationToken cancellationToken);

	Task<SettingsDto> GetSettingsAsync(CancellationToken cancellationToken);
}