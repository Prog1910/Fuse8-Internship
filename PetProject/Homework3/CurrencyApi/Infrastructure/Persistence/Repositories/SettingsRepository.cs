using Application.Persistence;

namespace Infrastructure.Persistence.Repositories;

public sealed class SettingsRepository : ISettingsRepository
{
	private readonly PublicDbContext _dbContext;

	public SettingsRepository(PublicDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public void UpdateDefaultCurrency(string defaultCurrency)
	{
		var query = _dbContext.Settings.FirstOrDefault();
		if (query is not null)
		{
			query.DefaultCurrency = defaultCurrency.ToString();
			_dbContext.Settings.Update(query);
		}
		_dbContext.SaveChanges();
	}

	public void UpdateCurrencyRoundCount(int currencyRoundCount)
	{
		var query = _dbContext.Settings.FirstOrDefault();
		if (query is not null)
			query.CurrencyRoundCount = currencyRoundCount;

		_dbContext.SaveChanges();
	}
}
