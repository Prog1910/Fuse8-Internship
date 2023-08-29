using Contracts;

namespace Application.Common.Interfaces;

public interface IInternalApi
{
	public Task<CurrencyResponse> GetCurrentCurrencyAsync();

	public Task<CurrencyResponse> GetCurrencyOnDateAsync(DateOnly date);

	public Task<CurrencyResponse> GetCurrentFavoriteCurrencyByNameAsync(string name);

	public Task<SettingsResponse> GetSettingsAsync();
}
