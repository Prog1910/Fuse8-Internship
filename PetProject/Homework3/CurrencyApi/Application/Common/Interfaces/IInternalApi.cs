using Contracts;
using Domain.Enums;

namespace Application.Common.Interfaces;

public interface IInternalApi
{
	public Task<CurrencyResponse> GetCurrentCurrencyAsync(CurrencyType currencyType);

	public Task<CurrencyResponse> GetCurrencyOnDateAsync(CurrencyType currencyType, DateOnly date);

	public Task<SettingsResponse> GetSettingsAsync();
}