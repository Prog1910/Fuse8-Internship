using Application.Public.Persistence;
using Domain.Aggregates;

namespace Infrastructure.Public.Persistence.Repositories;

public sealed class SettingsRepository : ISettingsRepository
{
	private readonly UserDbContext _context;
	private readonly SettingsCache _settings;

	public SettingsRepository(UserDbContext context)
	{
		_context = context;
		_settings = _context.Settings.SingleOrDefault() ?? throw new Exception("Settings not found.");
	}

	public string DefaultCurrencyCode
	{
		get => _settings.DefaultCurrencyCode;
		set
		{
			_settings.DefaultCurrencyCode = value;
			_context.SaveChanges();
		}
	}

	public int CurrencyRoundCount
	{
		get => _settings.CurrencyRoundCount;
		set
		{
			_settings.CurrencyRoundCount = value;
			_context.SaveChanges();
		}
	}
}