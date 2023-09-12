using PublicApi.Application.Interfaces.Rest;
using PublicApi.Domain.Persistence;
using Shared.Domain.Enums;

namespace PublicApi.Infrastructure.Services.Rest;

public sealed class SettingsService : ISettingsService
{
	private readonly UserDbContext _userDbContext;

	public SettingsService(UserDbContext userDbContext)
	{
		_userDbContext = userDbContext;
	}

	public CurrencyType DefaultCurrencyCode
		=> Enum.Parse<CurrencyType>(_userDbContext.Settings.SingleOrDefault()?.DefaultCurrencyCode ?? throw new Exception("Settings not found."));

	public async Task UpdateDefaultCurrencyCodeAsync(CurrencyType currencyCode, CancellationToken cancellationToken)
	{
		if (_userDbContext.Settings.SingleOrDefault() is not { } settings) throw new Exception("Settings not found.");
		settings.DefaultCurrencyCode = currencyCode.ToString();
		await _userDbContext.SaveChangesAsync(cancellationToken);
	}

	public int CurrencyRoundCount
		=> _userDbContext.Settings.SingleOrDefault()?.CurrencyRoundCount ?? throw new Exception("Settings not found.");

	public async Task UpdateCurrencyRoundCountAsync(int currencyRoundCount, CancellationToken cancellationToken)
	{
		if (currencyRoundCount is > 15 or < 0) throw new Exception("Rounding digits must be between 0 and 15");
		if (_userDbContext.Settings.SingleOrDefault() is not { } settings) throw new Exception("Settings not found.");

		settings.CurrencyRoundCount = currencyRoundCount;
		await _userDbContext.SaveChangesAsync(cancellationToken);
	}
}
