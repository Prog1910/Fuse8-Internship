using CurrencyApi.Application.Common.Services.Common.Dtos;
using CurrencyApi.Domain.Enums;

namespace CurrencyApi.Application.Common.Interfaces;

public interface ICachedCurrencyApi
{
    Task<CurrencyDto> GetCurrentCurrencyAsync(CurrencyType currencyType, CancellationToken cancellationToken);

    Task<CurrencyDto> GetCurrencyOnDateAsync(CurrencyType currencyType, DateOnly date, CancellationToken cancellationToken);

    Task<SettingsDto> GetSettingsAsync(CancellationToken cancellationToken);
}