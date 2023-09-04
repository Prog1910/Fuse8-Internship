using Application.Internal.Interfaces.Rest;
using Application.Internal.Persistence;
using Domain.Aggregates;
using Domain.Enums;
using Domain.Errors;
using Domain.Options;
using Infrastructure.Internal.Persistence;
using Microsoft.EntityFrameworkCore;
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
		var cacheTask = _taskRepo.GetCacheTaskById(cacheTaskId);
		var strategy = _curDbContext.Database.CreateExecutionStrategy();
		await strategy.ExecuteAsync(async () =>
		{
			await using var transaction = await _curDbContext.Database.BeginTransactionAsync(cancellationToken);
			try
			{
				cacheTask.Status = CacheTaskStatus.InProgress;
				await _curDbContext.SaveChangesAsync(cancellationToken);
				await transaction.CreateSavepointAsync("InProgress", cancellationToken);
			}
			catch (Exception e)
			{
				cacheTask.Status = CacheTaskStatus.CompletedWithError;
				await _curDbContext.SaveChangesAsync(cancellationToken);
				_logger.LogError(e, message: "An error occurred.");
				await transaction.RollbackToSavepointAsync("InProgress", cancellationToken);
			}
			try
			{
				if (_currencyRepo.GetAllCurrenciesOnDates()?.ToList() is { } currenciesOnDates)
				{
					var newBaseCurrencyCode = cacheTask.BaseCurrencyCode;
					await RecalculateCurrenciesOnDatesAsync(currenciesOnDates, newBaseCurrencyCode, cancellationToken);
					cacheTask.Status = CacheTaskStatus.CompletedSuccessfully;
					await _curDbContext.SaveChangesAsync(cancellationToken);
					await transaction.CreateSavepointAsync("CompletedSuccessfully", cancellationToken);
					_options.BaseCurrencyCode = newBaseCurrencyCode;
					await transaction.CommitAsync(cancellationToken);
				}
			}
			catch (Exception e)
			{
				cacheTask.Status = CacheTaskStatus.CompletedWithError;
				await _curDbContext.SaveChangesAsync(cancellationToken);
				_logger.LogError(e, message: "An error occurred.");
			}
		});
	}

	public async Task<Currency?> RecalculateCurrencyAsync(string currencyCode, string newBaseCurrencyCode, string oldBaseCurrencyCode, List<Currency> currencies, CancellationToken cancellationToken)
		=> await Task.Run(() =>
		{
			var currency = currencies.SingleOrDefault(c => c.Code.Equals(currencyCode)) ?? throw new CurrencyNotFoundException();
			if (currencyCode.Equals(oldBaseCurrencyCode)) return currency;

			var baseCurrency = currencies.SingleOrDefault(c => c.Code.Equals(newBaseCurrencyCode)) ?? throw new CurrencyNotFoundException();
			var recalculatedCurrency = currency with { Value = currency.Value / baseCurrency.Value };

			return recalculatedCurrency;
		}, cancellationToken);

	private async Task RecalculateCurrenciesOnDatesAsync(List<CurrenciesOnDateCache> currenciesOnDates, string newBaseCurrencyCode, CancellationToken cancellationToken)
	{
		foreach (var currenciesOnDate in currenciesOnDates)
		{
			var oldBaseCurrencyCode = currenciesOnDate.BaseCurrencyCode;
			var recalculatedCurrencies = new CurrenciesOnDateCache
			{
				LastUpdatedAt = currenciesOnDate.LastUpdatedAt,
				BaseCurrencyCode = newBaseCurrencyCode,
				Currencies = new List<Currency>(currenciesOnDate.Currencies.Count)
			};
			foreach (var currency in currenciesOnDate.Currencies)
			{
				var recalculatedCurrency = await RecalculateCurrencyAsync(currency.Code, newBaseCurrencyCode, oldBaseCurrencyCode, currenciesOnDate.Currencies, cancellationToken)
				                           ?? throw new CurrencyNotFoundException();
				recalculatedCurrencies.Currencies.Add(recalculatedCurrency);
			}
			_currencyRepo.UpdateCurrenciesOnDate(recalculatedCurrencies);
		}
	}
}