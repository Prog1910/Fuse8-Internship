﻿using InternalApi.Application.Interfaces.Rest;
using InternalApi.Domain.Aggregates;
using InternalApi.Domain.Enums;
using InternalApi.Domain.Options;
using InternalApi.Domain.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Application.Dtos;
using Shared.Domain.Enums;
using Shared.Domain.Errors;

namespace InternalApi.Application.Services.Rest;

public sealed class CacheCurrencyService : ICacheCurrencyApi
{
	private readonly CurDbContext _curDbContext;
	private readonly ICurrencyApi _currencyService;
	private readonly InternalApiOptions _options;

	public CacheCurrencyService(IOptions<InternalApiOptions> options, CurDbContext curDbContext, ICurrencyApi currencyService)
	{
		_options = options.Value;
		_curDbContext = curDbContext;
		_currencyService = currencyService;
	}

	public async Task<CurrencyDto> GetCurrentCurrencyAsync(CurrencyType defaultCurrencyCode, CancellationToken cancellationToken)
	{
		string defaultCurrencyCodeStr = defaultCurrencyCode.ToString();
		IEnumerable<Currency>? currencies = await TryGetCurrenciesFromCacheByBaseCodeAsync(_options.BaseCurrencyCode, default, cancellationToken);
		Currency currency = currencies?.SingleOrDefault(c => c.Code.Equals(defaultCurrencyCodeStr)) ?? throw new CurrencyNotFoundException();

		return currency.Adapt<CurrencyDto>();
	}

	public async Task<CurrencyDto> GetCurrencyOnDateAsync(CurrencyType defaultCurrencyCode, DateOnly date, CancellationToken cancellationToken)
	{
		if (date.Equals(DateOnly.FromDateTime(DateTime.UtcNow))) return await GetCurrentCurrencyAsync(defaultCurrencyCode, cancellationToken);

		string defaultCurrencyCodeStr = defaultCurrencyCode.ToString();
		IEnumerable<Currency>? currencies = await TryGetCurrenciesFromCacheByBaseCodeAsync(_options.BaseCurrencyCode, date, cancellationToken);
		Currency currency = currencies?.SingleOrDefault(c => c.Code.Equals(defaultCurrencyCodeStr)) ?? throw new CurrencyNotFoundException();

		return currency.Adapt<CurrencyDto>();
	}

	public async Task<CurrencyDto> GetCurrencyByFavoritesAsync(CurrencyType favoriteCurrencyCode, CurrencyType favoriteBaseCurrencyCode, DateOnly? date,
		CancellationToken cancellationToken)
	{
		string baseCurrencyCode = _options.BaseCurrencyCode;
		string favoriteCurrencyCodeStr = favoriteCurrencyCode.ToString();
		string favoriteBaseCurrencyCodeStr = favoriteBaseCurrencyCode.ToString();
		List<Currency> currencies = (await GetCurrenciesFromCacheByBaseCurrencyCode(baseCurrencyCode, date))?.ToList() ?? throw new CurrencyNotFoundException();
		Currency currency = currencies.SingleOrDefault(c => c.Code.Equals(favoriteCurrencyCodeStr)) ?? throw new CurrencyNotFoundException();
		if (baseCurrencyCode.Equals(favoriteBaseCurrencyCodeStr) is false)
		{
			Currency baseCurrency = currencies.SingleOrDefault(c => c.Code.Equals(favoriteBaseCurrencyCodeStr)) ?? throw new CurrencyNotFoundException();
			currency.Value /= baseCurrency.Value;
		}

		return currency.Adapt<CurrencyDto>();
	}

	public async Task<SettingsDto> GetSettingsAsync(CancellationToken cancellationToken)
	{
		Settings settings = await _currencyService.GetSettingsAsync(cancellationToken);

		return settings.Adapt<SettingsDto>();
	}

	private async Task<IEnumerable<Currency>?> GetCurrenciesFromCacheByBaseCurrencyCode(string baseCurrencyCode, DateOnly? date)
	{
		IQueryable<CurrenciesOnDateCache> queryBaseCurrency = _curDbContext.CurrenciesOnDates.Where(cod => cod.BaseCurrencyCode.Equals(baseCurrencyCode));
		IQueryable<CurrenciesOnDateCache> queryDate = date is { } dateOnly
			? queryBaseCurrency.Where(cod => DateOnly.FromDateTime(cod.LastUpdatedAt.ToUniversalTime()).Equals(dateOnly))
			: queryBaseCurrency.Where(cod => cod.LastUpdatedAt.ToUniversalTime().AddHours(2) > DateTime.UtcNow);

		CurrenciesOnDateCache? currenciesOnDate = await queryDate.FirstOrDefaultAsync();

		return currenciesOnDate?.Currencies;
	}

	private async Task<IEnumerable<Currency>?> TryGetCurrenciesFromCacheByBaseCodeAsync(string baseCurrencyCode, DateOnly? date,
		CancellationToken cancellationToken)
	{
		IEnumerable<Currency>? currencies = await GetCurrenciesFromCacheByBaseCurrencyCode(baseCurrencyCode, date);
		if (currencies is not null) return currencies;

		if (_curDbContext.CacheTasks.Any(t => t.Status == CacheTaskStatus.Created || t.Status == CacheTaskStatus.InProgress))
		{
			await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
			if (_curDbContext.CacheTasks.Any(t => t.Status == CacheTaskStatus.Created || t.Status == CacheTaskStatus.InProgress))
			{
				throw new Exception("An error occurred while recalculating cache.");
			}
		}
		else if (_curDbContext.CacheTasks.Any(t => t.Status == CacheTaskStatus.InProgress) is false)
		{
			if (date.HasValue)
			{
				CurrenciesOnDate currenciesOnDate = await _currencyService.GetAllCurrenciesOnDateAsync(baseCurrencyCode, date.Value, cancellationToken);
				currencies = currenciesOnDate.Currencies;
				_curDbContext.CurrenciesOnDates.Add(new CurrenciesOnDateCache
				{
					LastUpdatedAt = currenciesOnDate.LastUpdatedAt.Date.ToUniversalTime(),
					BaseCurrencyCode = baseCurrencyCode,
					Currencies = currencies.ToList()
				});
			}
			else
			{
				currencies = await _currencyService.GetAllCurrentCurrenciesAsync(baseCurrencyCode, cancellationToken);
				_curDbContext.CurrenciesOnDates.Add(new CurrenciesOnDateCache
				{
					LastUpdatedAt = DateTime.UtcNow,
					BaseCurrencyCode = baseCurrencyCode,
					Currencies = currencies.ToList()
				});
			}

			await _curDbContext.SaveChangesAsync(cancellationToken);
		}

		return currencies;
	}
}
