using Application.Persistence;
using Domain.Aggregates.CurrenciesOnDateAggregate;
using Domain.Aggregates.CurrencyAggregate;

namespace Infrastructure.Persistence.Repositories;

public sealed class CurrencyRepository : ICurrencyRepository
{
	private readonly SummerSchoolDbContext _dbContext;

	public CurrencyRepository(SummerSchoolDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public void AddCurrencies(string baseCurrency, Currency[] currencies, DateTime? date = null)
	{
		var dateTime = date?.ToUniversalTime() ?? DateTime.UtcNow;
		if (dateTime.Date.Equals(date?.Date))
			RemoveExpiredCache(baseCurrency, dateTime);

		var currenciesToCache = new CachedCurrencies()
		{
			LastUpdatedAt = dateTime,
			BaseCurrency = baseCurrency,
			Currencies = currencies.ToList()
		};

		_dbContext.Add(currenciesToCache);
		_dbContext.SaveChanges();
	}

	private void RemoveExpiredCache(string baseCurrency, DateTime date)
	{
		var queryBaseCurrency = _dbContext.CurrenciesOnDate.Where(ccod => ccod.BaseCurrency.Equals(baseCurrency));
		var queryDate = queryBaseCurrency.Where(ccod => DateOnly.FromDateTime(ccod.LastUpdatedAt.ToUniversalTime()).Equals(date));
		_dbContext.CurrenciesOnDate.RemoveRange(queryDate);
	}

	public Currency[]? GetCurrencies(string baseCurrency, DateOnly? date = null)
	{
		var queryBaseCurrency = _dbContext.CurrenciesOnDate.Where(ccod => ccod.BaseCurrency.Equals(baseCurrency));
		var queryDate = date is DateOnly dateOnly
			? queryBaseCurrency.Where(ccod => DateOnly.FromDateTime(ccod.LastUpdatedAt.ToUniversalTime()).Equals(dateOnly))
			: queryBaseCurrency.Where(ccod => ccod.LastUpdatedAt.ToUniversalTime().AddHours(2) > DateTime.UtcNow);
		var currencies = queryDate.FirstOrDefault()?.Currencies.ToArray();

		return currencies;
	}
}