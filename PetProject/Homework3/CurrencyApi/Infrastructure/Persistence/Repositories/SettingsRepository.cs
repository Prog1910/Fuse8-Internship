using Application.Persistence;

namespace Infrastructure.Persistence.Repositories;

public sealed class SettingsRepository : ISettingsRepository
{
	private readonly PublicDbContext _context;

	public SettingsRepository(PublicDbContext context)
	{
		_context = context;
	}

	public void UpdateDefaultCurrency(string defaultCurrency)
	{
		var settings = _context.Settings.FirstOrDefault();
		if (settings is not null)
		{
			settings.DefaultCurrency = defaultCurrency.ToString();
			_context.SaveChanges();
		}
	}

	public void UpdateCurrencyRoundCount(int currencyRoundCount)
	{
		var settings = _context.Settings.FirstOrDefault();
		if (settings is not null)
		{
			settings.CurrencyRoundCount = currencyRoundCount;
			_context.SaveChanges();
		}
	}
}
