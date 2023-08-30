using Application.Common.Interfaces.Rest;
using Application.Persistence;

namespace Infrastructure.Services.Rest;

public sealed class SettingsReaderService : ISettingsReaderService
{
	private readonly ISettingsRepository _settingsRepo;

	public SettingsReaderService(ISettingsRepository settingsRepo)
	{
		_settingsRepo = settingsRepo;
	}

	public string DefaultCurrency
		=> _settingsRepo?.DefaultCurrency ?? throw new Exception("Default currency not found");

	public int CurrencyRoundCount
		=> _settingsRepo.CurrencyRoundCount ?? throw new Exception("Currency round count not found");
}