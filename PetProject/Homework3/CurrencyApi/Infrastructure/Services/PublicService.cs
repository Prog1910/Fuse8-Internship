using Application.Common.Interfaces;
using Application.Persistence;
using Domain.Aggregates.CachedFavoriteCurrenciesAggregate;
using Domain.Enums;

namespace Infrastructure.Services;

public sealed class PublicService : IPublicApi
{
	private readonly ISettingsRepository _settingsRepo;
	private readonly IFavoriteCurrenciesRepository _favoritesRepo;

	public PublicService(ISettingsRepository settingsRepo, IFavoriteCurrenciesRepository favoritesRepo)
	{
		_settingsRepo = settingsRepo;
		_favoritesRepo = favoritesRepo;
	}

	public async Task UpdateDefaultCurrencyAsync(CurrencyType defaultCurrency)
	{
		await Task.CompletedTask;
		_settingsRepo.UpdateDefaultCurrency(defaultCurrency.ToString());
	}

	public async Task UpdateCurrencyRoundCountAsync(int currencyRoundCount)
	{
		await Task.CompletedTask;
		_settingsRepo.UpdateCurrencyRoundCount(currencyRoundCount);
	}

	public async Task<CachedFavoriteCurrency?> GetFavoriteCurrenciesByNameAsync(string name)
	{
		await Task.CompletedTask;
		var favorite = _favoritesRepo.GetFavoriteCurrencyByName(name);

		return favorite;
	}

	public async Task<List<CachedFavoriteCurrency>?> GetAllFavoriteCurrenciesAsync()
	{
		await Task.CompletedTask;
		var favorites = _favoritesRepo.GetAllFavoriteCurrencies();

		return favorites;
	}

	public async Task AddFavoriteCurrenciesAsync(CachedFavoriteCurrency favoriteCurrencies)
	{
		await Task.CompletedTask;
		_favoritesRepo.TryAddFavoriteCurrencies(favoriteCurrencies);
	}
	public async Task UpdateFavoriteCurrenciesByNameAsync(string name, CachedFavoriteCurrency favoriteCurrencies)
	{
		await Task.CompletedTask;
		_favoritesRepo.TryUpdateFavoriteCurrencyByName(name, favoriteCurrencies);
	}

	public async Task DeleteFavoriteCurrenciesByNameAsync(string name)
	{
		await Task.CompletedTask;
		_favoritesRepo.TryDeleteFavoriteCurrencyByName(name);
	}
}
