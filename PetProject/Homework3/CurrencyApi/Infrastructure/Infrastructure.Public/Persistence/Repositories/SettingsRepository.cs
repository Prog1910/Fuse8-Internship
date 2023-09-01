using Application.Public.Persistence;

namespace Infrastructure.Public.Persistence.Repositories;

public sealed class SettingsRepository : ISettingsRepository
{
	private readonly UserDbContext _context;

	public SettingsRepository(UserDbContext context)
	{
		_context = context;
	}

	public string? DefaultCurrency
	{
		get
		{
			var settings = _context.Settings.SingleOrDefault();

			return settings?.DefaultCurrency;
		}
	}

	public int? CurrencyRoundCount
	{
		get
		{
			var settings = _context.Settings.SingleOrDefault();

			return settings?.CurrencyRoundCount;
		}
	}

	public void UpdateDefaultCurrency(string defaultCurrency)
	{
		var settings = _context.Settings.SingleOrDefault();
		if (settings is not null)
		{
			settings.DefaultCurrency = defaultCurrency;
			_context.SaveChanges();
		}
	}

	public void UpdateCurrencyRoundCount(int currencyRoundCount)
	{
		var settings = _context.Settings.SingleOrDefault();
		if (settings is not null)
		{
			settings.CurrencyRoundCount = currencyRoundCount;
			_context.SaveChanges();
		}
	}
}