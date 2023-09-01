using Application.Internal.Persistence;
using Domain.Aggregates;

namespace Infrastructure.Internal.Persistence.Repositories;

public sealed class CurrenciesRepository : ICurrenciesRepository
{
	private readonly CurDbContext _dbContext;

	public CurrenciesRepository(CurDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public void AddCurrencies(string baseCurrency, Currency[] currencies, DateTime? date = null)
	{
		var dateTime = date?.ToUniversalTime() ?? DateTime.UtcNow;
		var currenciesToCache = new CachedCurrencies
		{
			LastUpdatedAt = dateTime,
			BaseCurrency = baseCurrency,
			Currencies = currencies.ToList()
		};

		_dbContext.AddRange(currenciesToCache);
		_dbContext.SaveChanges();
	}

	public Currency[]? GetCurrencies(string baseCurrency, DateOnly? date = null)
	{
		var queryBaseCurrency = _dbContext.CurrenciesOnDate.Where(ccod => ccod.BaseCurrency.Equals(baseCurrency));
		if (queryBaseCurrency.Any())
		{
			var queryDate = date is DateOnly dateOnly
				? queryBaseCurrency.Where(ccod => DateOnly.FromDateTime(ccod.LastUpdatedAt.ToLocalTime()).Equals(dateOnly))
				: queryBaseCurrency.Where(ccod => ccod.LastUpdatedAt.ToUniversalTime().AddHours(2) > DateTime.UtcNow);
			var currencies = queryDate.FirstOrDefault()?.Currencies.ToArray();

			return currencies;
		}

		return null;
	}
}