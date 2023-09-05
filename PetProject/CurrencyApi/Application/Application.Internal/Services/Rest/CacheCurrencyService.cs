using Application.Internal.Interfaces.Rest;
using Application.Internal.Persistence;
using Application.Shared.Dtos;
using Domain.Aggregates;
using Domain.Enums;
using Domain.Errors;
using Domain.Options;
using Mapster;
using Microsoft.Extensions.Options;

namespace Application.Internal.Services.Rest;

public sealed class CacheCurrencyService : ICacheCurrencyApi
{
	private readonly ICurrencyApi _currencyService;
	private readonly InternalApiOptions _options;
	private readonly ICurrencyRepository _currencyRepo;
	private readonly ITaskRepository _taskRepo;

	public CacheCurrencyService(IOptions<InternalApiOptions> options, ICurrencyApi currencyService, ICurrencyRepository currencyRepo, ITaskRepository taskRepo)
	{
		_options = options.Value;
		_currencyService = currencyService;
		_currencyRepo = currencyRepo;
		_taskRepo = taskRepo;
	}

	public async Task<CurrencyDto> GetCurrentCurrencyAsync(CurrencyType defaultCurrencyCode, CancellationToken cancellationToken)
	{
		var defaultCurrencyCodeStr = defaultCurrencyCode.ToString();
		var currencies = await TryGetCurrenciesFromCacheByBaseCodeAsync(_options.BaseCurrencyCode, date: default, cancellationToken);
		var currency = currencies?.SingleOrDefault(c => c.Code.Equals(defaultCurrencyCodeStr)) ?? throw new CurrencyNotFoundException();

		return currency.Adapt<CurrencyDto>();
	}

	public async Task<CurrencyDto> GetCurrencyOnDateAsync(CurrencyType defaultCurrencyCode, DateOnly date, CancellationToken cancellationToken)
	{
		if (date.Equals(DateOnly.FromDateTime(DateTime.UtcNow))) return await GetCurrentCurrencyAsync(defaultCurrencyCode, cancellationToken);

		var defaultCurrencyCodeStr = defaultCurrencyCode.ToString();
		var currencies = await TryGetCurrenciesFromCacheByBaseCodeAsync(_options.BaseCurrencyCode, date, cancellationToken);
		var currency = currencies?.SingleOrDefault(c => c.Code.Equals(defaultCurrencyCodeStr)) ?? throw new CurrencyNotFoundException();

		return currency.Adapt<CurrencyDto>();
	}

	public async Task<CurrencyDto> GetCurrencyByFavoritesAsync(CurrencyType favoriteCurrencyCode, CurrencyType favoriteBaseCurrencyCode, DateOnly? date, CancellationToken cancellationToken)
	{
		return await Task.Run(() =>
		{
			var baseCurrencyCode = _options.BaseCurrencyCode;
			var favoriteCurrencyCodeStr = favoriteCurrencyCode.ToString();
			var favoriteBaseCurrencyCodeStr = favoriteBaseCurrencyCode.ToString();
			var currencies = GetCurrenciesFromCacheByBaseCurrencyCode(baseCurrencyCode, date)?.ToList() ?? throw new CurrencyNotFoundException();
			var currency = currencies.SingleOrDefault(c => c.Code.Equals(favoriteCurrencyCodeStr)) ?? throw new CurrencyNotFoundException();
			if (baseCurrencyCode.Equals(favoriteBaseCurrencyCodeStr) is false)
			{
				var baseCurrency = currencies.SingleOrDefault(c => c.Code.Equals(favoriteBaseCurrencyCodeStr)) ?? throw new CurrencyNotFoundException();
				currency.Value /= baseCurrency.Value;
			}
			return currency.Adapt<CurrencyDto>();
		}, cancellationToken);
	}

	public async Task<SettingsDto> GetSettingsAsync(CancellationToken cancellationToken)
	{
		var settings = await _currencyService.GetSettingsAsync(cancellationToken);

		return settings.Adapt<SettingsDto>();
	}

	private IEnumerable<Currency>? GetCurrenciesFromCacheByBaseCurrencyCode(string baseCurrencyCode, DateOnly? date)
		=> _currencyRepo.GetCurrenciesByBaseCode(baseCurrencyCode, date);

	private async Task<IEnumerable<Currency>?> TryGetCurrenciesFromCacheByBaseCodeAsync(string baseCurrencyCode, DateOnly? date, CancellationToken cancellationToken)
	{
		var currencies = GetCurrenciesFromCacheByBaseCurrencyCode(baseCurrencyCode, date);
		if (currencies is not null) return currencies;

		if (_taskRepo.GetAllTasks().Any(t => t.Status is CacheTaskStatus.Created or CacheTaskStatus.InProgress))
		{
			await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
			if (_taskRepo.GetAllTasks().Any()) throw new Exception("An error occurred while recalculating cache.");
		}
		else if (_taskRepo.GetAllTasks().Any(t => t.Status is CacheTaskStatus.InProgress) is false)
		{
			if (date.HasValue)
			{
				var currenciesOnDate = await _currencyService.GetAllCurrenciesOnDateAsync(baseCurrencyCode, date.Value, cancellationToken);
				var lastUpdateAt = currenciesOnDate.LastUpdatedAt.Date.ToUniversalTime();
				currencies = currenciesOnDate.Currencies;
				_currencyRepo.AddCurrenciesByBaseCode(baseCurrencyCode, currencies, lastUpdateAt);
			}
			else
			{
				currencies = await _currencyService.GetAllCurrentCurrenciesAsync(baseCurrencyCode, cancellationToken);
				_currencyRepo.AddCurrenciesByBaseCode(baseCurrencyCode, currencies);
			}
		}

		return currencies;
	}
}