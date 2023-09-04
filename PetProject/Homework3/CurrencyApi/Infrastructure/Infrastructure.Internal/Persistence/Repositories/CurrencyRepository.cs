using Application.Internal.Persistence;
using Domain.Aggregates;

namespace Infrastructure.Internal.Persistence.Repositories;

public sealed class CurrencyRepository : ICurrencyRepository
{
	private readonly CurDbContext _dbContext;

	public CurrencyRepository(CurDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public void AddCurrenciesByBaseCode(string baseCurrency, IEnumerable<Currency> currencies, DateTime? date = default)
	{
		var dateTime = date?.ToUniversalTime() ?? DateTime.UtcNow;
		var currenciesToCache = new CurrenciesOnDateCache
		{
			LastUpdatedAt = dateTime,
			BaseCurrencyCode = baseCurrency,
			Currencies = currencies.ToList()
		};

		_dbContext.AddRange(currenciesToCache);
		_dbContext.SaveChanges();
	}

	public IEnumerable<Currency>? GetCurrenciesByBaseCode(string baseCurrency, DateOnly? date = null)
	{
		var queryBaseCurrency = _dbContext.CurrenciesOnDate.Where(cod => cod.BaseCurrencyCode.Equals(baseCurrency));
		if (queryBaseCurrency.Any() is false) return null;

		var queryDate = date is { } dateOnly
			? queryBaseCurrency.Where(cod => DateOnly.FromDateTime(cod.LastUpdatedAt.ToUniversalTime()).Equals(dateOnly))
			: queryBaseCurrency.Where(cod => cod.LastUpdatedAt.ToLocalTime().AddHours(2) > DateTime.Now);
		var currencies = queryDate.FirstOrDefault()?.Currencies;

		return currencies;
	}

	public IEnumerable<CurrenciesOnDateCache> GetAllCurrenciesOnDates(DateOnly? date = null)
	{
		var currenciesOnDate = _dbContext.CurrenciesOnDate.OrderByDescending(cod => cod.LastUpdatedAt).AsEnumerable();

		return currenciesOnDate;
	}

	public void UpdateCurrenciesOnDate(CurrenciesOnDateCache currenciesOnDate)
	{
		_dbContext.CurrenciesOnDate.Update(currenciesOnDate);
		_dbContext.SaveChanges();
	}
}