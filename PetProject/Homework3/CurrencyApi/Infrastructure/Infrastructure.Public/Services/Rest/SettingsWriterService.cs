using Application.Public.Interfaces.Rest;
using Application.Public.Persistence;
using Domain.Enums;

namespace Infrastructure.Public.Services.Rest;

public sealed class SettingsWriterService : ISettingsWriterService
{
	private readonly ISettingsRepository _settingsRepo;

	public SettingsWriterService(ISettingsRepository settingsRepo)
	{
		_settingsRepo = settingsRepo;
	}

	public async Task UpdateDefaultCurrency(CurrencyType defaultCurrency)
	{
		await Task.CompletedTask;
		_settingsRepo.UpdateDefaultCurrency(defaultCurrency.ToString());
	}

	public async Task UpdateCurrencyRoundCount(int currencyRoundCount)
	{
		await Task.CompletedTask;
		if (currencyRoundCount < 0) throw new Exception("Currency round count must be non-negative.");

		_settingsRepo.UpdateCurrencyRoundCount(currencyRoundCount);
	}
}