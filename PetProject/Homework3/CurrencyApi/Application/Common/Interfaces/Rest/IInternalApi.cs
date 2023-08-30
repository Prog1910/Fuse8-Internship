using Contracts;

namespace Application.Common.Interfaces.Rest;

public interface IInternalApi
{
    public Task<CurrencyResponse> GetCurrentCurrencyAsync();

    public Task<CurrencyResponse> GetCurrencyOnDateAsync(DateOnly date);

    public Task<CurrencyResponse> GetCurrentFavoriteCurrencyByNameAsync(string name);

    public Task<CurrencyResponse> GetFavoriteCurrencyOnDateByNameAsync(string name, DateOnly date);

    public Task<SettingsResponse> GetSettingsAsync();
}