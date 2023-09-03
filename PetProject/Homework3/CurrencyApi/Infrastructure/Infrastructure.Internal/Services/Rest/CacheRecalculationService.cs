using Application.Internal.Interfaces.Rest;
using Application.Internal.Persistence;
using Domain.Aggregates;
using Domain.Enums;
using Domain.Errors;
using Domain.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Internal.Services.Rest;

public sealed class CacheRecalculationService : ICacheRecalculationApi
{
	private readonly ICurrencyRepository _currencyRepo;
	private readonly ITaskRepository _taskRepo;
	private readonly InternalApiOptions _options;
	private readonly ILogger<ICacheRecalculationApi> _logger;

	public CacheRecalculationService(IOptionsSnapshot<InternalApiOptions> options, ITaskRepository taskRepo, ICurrencyRepository currencyRepo, ILogger<ICacheRecalculationApi> logger)
	{
		_options = options.Value;
		_taskRepo = taskRepo;
		_currencyRepo = currencyRepo;
		_logger = logger;
	}

	public async Task RecalculateCacheAsync(Guid cacheTaskId, CancellationToken cancellationToken)
	{
		var cacheTask = _taskRepo.GetCacheTaskById(cacheTaskId);
		try
		{
			var newBaseCurrencyCode = cacheTask.BaseCurrencyCode;
			_taskRepo.UpdateCacheTask(cacheTask with { Status = CacheTaskStatus.InProgress });

			if (_currencyRepo.GetAllCurrenciesOnDates()?.ToList() is { } currenciesOnDates)
			{
				await RecalculateCurrenciesOnDatesAsync(cancellationToken, currenciesOnDates, newBaseCurrencyCode);
				_options.BaseCurrencyCode = newBaseCurrencyCode;
				_taskRepo.UpdateCacheTask(cacheTask with { Status = CacheTaskStatus.CompletedSuccessfully });
			}
		}
		catch (Exception exception)
		{
			_taskRepo.UpdateCacheTask(cacheTask with { Status = CacheTaskStatus.CompletedWithError });
			_logger.LogError(exception, message: "An error occurred.");
		}
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

	private async Task RecalculateCurrenciesOnDatesAsync(CancellationToken cancellationToken, List<CurrenciesOnDateCache> currenciesOnDates, string newBaseCurrencyCode)
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