using Application.Public.Interfaces.Rest;
using Application.Public.Persistence;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Public.Services.Rest;

public sealed class SettingsService : ISettingsService
{
	private readonly IUserDbContext _userDbContext;

	public SettingsService(IUserDbContext userDbContext)
	{
		_userDbContext = userDbContext;
	}

	public CurrencyType DefaultCurrencyCode
	{
		get => Enum.Parse<CurrencyType>(_userDbContext.Settings.SingleOrDefault()?.DefaultCurrencyCode ?? throw new Exception("Settings not found."));
		set
		{
			if (_userDbContext.Settings.SingleOrDefault() is not { } settings) throw new Exception("Settings not found.");

			settings.DefaultCurrencyCode = value.ToString();
			_userDbContext.SaveChangesAsync();
		}
	}

	public int CurrencyRoundCount
	{
		get => _userDbContext.Settings.SingleOrDefault()?.CurrencyRoundCount ?? throw new Exception("Settings not found.");
		set
		{
			if (value < 0) throw new Exception("Currency round count must be non-negative.");
			if (_userDbContext.Settings.SingleOrDefault() is not { } settings) throw new Exception("Settings not found.");

			settings.CurrencyRoundCount = value;
			_userDbContext.SaveChangesAsync();
		}
	}
}