using Application.Internal.Interfaces.Rest;
using Domain.Aggregates;
using Domain.Enums;
using Domain.Errors;
using Domain.Options;
using Infrastructure.Internal.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Internal.Services.Rest;

public sealed class CacheRecalculationService : ICacheRecalculationService
{
	private readonly CurDbContext _curDbContext;
	private readonly ILogger<ICacheRecalculationService> _logger;
	private readonly InternalApiOptions _options;

	public CacheRecalculationService(IOptions<InternalApiOptions> options, CurDbContext curDbContext, ILogger<ICacheRecalculationService> logger)
	{
		_options = options.Value;
		_curDbContext = curDbContext;
		_logger = logger;
	}

	public async Task RecalculateCacheAsync(Guid cacheTaskId, CancellationToken cancellationToken)
	{
		CacheTask cacheTask = _curDbContext.CacheTasks.SingleOrDefault(t => t.Id.Equals(cacheTaskId)) ?? throw new Exception("Cache task not found.");
		try
		{
			cacheTask.Status = CacheTaskStatus.InProgress;
			await _curDbContext.SaveChangesAsync(cancellationToken);
			if (_curDbContext.CurrenciesOnDates.OrderByDescending(cod => cod.LastUpdatedAt).AsEnumerable().ToList() is { } currenciesOnDates)
			{
				string newBaseCurrencyCode = cacheTask.BaseCurrencyCode;
				await RecalculateCurrencyCacheAsync(currenciesOnDates, newBaseCurrencyCode, cancellationToken);
				_options.BaseCurrencyCode = newBaseCurrencyCode;
				cacheTask.Status = CacheTaskStatus.CompletedSuccessfully;
				await _curDbContext.SaveChangesAsync(cancellationToken);
			}
		}
		catch (Exception e)
		{
			cacheTask.Status = CacheTaskStatus.CompletedWithError;
			await _curDbContext.SaveChangesAsync(cancellationToken);
			_logger.LogError(e, message: "An error occurred while recalculating cache.");
		}
	}

	private async Task RecalculateCurrencyCacheAsync(List<CurrenciesOnDateCache> currenciesOnDates, string newBaseCurrencyCode, CancellationToken cancellationToken)
	{
		await Task.Run(() =>
		{
			foreach (CurrenciesOnDateCache currenciesOnDate in currenciesOnDates)
			{
				if (currenciesOnDate.BaseCurrencyCode.Equals(newBaseCurrencyCode)) continue;

				// За 2002-ой год маната (AZN) нет, поэтому относительно него пересчитать кэш за 2002-ой год не получится 
				if (currenciesOnDate.Currencies.SingleOrDefault(c => c.Code.Equals(newBaseCurrencyCode))?.Value is not { } relativeBaseCurrencyRate) throw new CurrencyNotFoundException();

				CurrenciesOnDateCache newCurrenciesOnDate = new CurrenciesOnDateCache
				{
					LastUpdatedAt = currenciesOnDate.LastUpdatedAt,
					BaseCurrencyCode = newBaseCurrencyCode,
					Currencies = currenciesOnDate.Currencies.Select(currency =>
					{
						currency.Value /= relativeBaseCurrencyRate;
						return currency;
					}).ToList()
				};
				_curDbContext.CurrenciesOnDates.Remove(currenciesOnDate);
				_curDbContext.SaveChanges();
				_curDbContext.Add(newCurrenciesOnDate);
				_curDbContext.SaveChanges();
			}
		}, cancellationToken);
	}
}