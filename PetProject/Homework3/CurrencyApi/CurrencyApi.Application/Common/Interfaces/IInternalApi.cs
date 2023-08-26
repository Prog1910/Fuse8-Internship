using CurrencyApi.Contracts;
using CurrencyApi.Domain.Enums;

namespace CurrencyApi.Application.Common.Interfaces;

public interface IInternalApi
{
    public Task<CurrencyResponse> GetCurrentCurrencyAsync(CurrencyType currencyType);

    public Task<CurrencyResponse> GetCurrencyOnDateAsync(CurrencyType currencyType, DateOnly date);

    public Task<SettingsResponse> GetSettingsAsync();
}