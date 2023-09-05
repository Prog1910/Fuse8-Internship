using Application.Internal.Interfaces.Rest;
using Application.Internal.Persistence;
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
	private readonly ICurrencyRepository _currencyRepo;
	private readonly ITaskRepository _taskRepo;
	private readonly InternalApiOptions _options;
	private readonly ILogger<ICacheRecalculationService> _logger;
	private readonly CurDbContext _curDbContext;

	public CacheRecalculationService(IOptions<InternalApiOptions> options, ITaskRepository taskRepo, ICurrencyRepository currencyRepo, ILogger<ICacheRecalculationService> logger, CurDbContext curDbContext)
	{
		_options = options.Value;
		_taskRepo = taskRepo;
		_currencyRepo = currencyRepo;
		_logger = logger;
		_curDbContext = curDbContext;
	}

	public async Task RecalculateCacheAsync(Guid cacheTaskId, CancellationToken cancellationToken)
	{
		var cacheTask = await _taskRepo.GetCacheTaskById(cacheTaskId) ?? throw new Exception("Cache task not found.");
		try
		{
			cacheTask.Status = CacheTaskStatus.InProgress;
			await _curDbContext.SaveChangesAsync(cancellationToken);
			if (_currencyRepo.GetAllCurrenciesOnDates()?.ToList() is { } currenciesOnDates)
			{
				var newBaseCurrencyCode = cacheTask.BaseCurrencyCode;
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
			foreach (var currenciesOnDate in currenciesOnDates)
			{
				if (currenciesOnDate.BaseCurrencyCode.Equals(newBaseCurrencyCode)) continue;

				// За 2002-ой год маната (AZN) нет, поэтому относительно него пересчитать кэш за 2002-ой год не получится 
				// var relativeBaseCurrencyRate = currenciesOnDate.Currencies.SingleOrDefault(c => c.Code.Equals(newBaseCurrencyCode))?.Value ?? throw new CurrencyNotFoundException();
				if (currenciesOnDate.Currencies.SingleOrDefault(c => c.Code.Equals(newBaseCurrencyCode))?.Value is not { } relativeBaseCurrencyRate) continue;

				var newCurrenciesOnDate = new CurrenciesOnDateCache
				{
					LastUpdatedAt = currenciesOnDate.LastUpdatedAt,
					BaseCurrencyCode = newBaseCurrencyCode,
					Currencies = currenciesOnDate.Currencies.Select(currency =>
					{
						currency.Value /= relativeBaseCurrencyRate;
						return currency;
					}).ToList()
				};
				_curDbContext.CurrenciesOnDate.Remove(currenciesOnDate);
				_curDbContext.SaveChanges();
				_curDbContext.Add(newCurrenciesOnDate);
				_curDbContext.SaveChanges();
			}
		}, cancellationToken);
	}
}