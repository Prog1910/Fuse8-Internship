using Application.Public.Interfaces.Rest;
using Application.Public.Persistence;
using Domain.Enums;

namespace Infrastructure.Public.Services.Rest;

public sealed class SettingsService : ISettingsService
{
	private readonly ISettingsRepository _settingsRepo;

	public SettingsService(ISettingsRepository settingsRepo)
	{
		_settingsRepo = settingsRepo;
	}

	public CurrencyType DefaultCurrencyCode
	{
		get => Enum.Parse<CurrencyType>(_settingsRepo.DefaultCurrencyCode);
		set => _settingsRepo.DefaultCurrencyCode = value.ToString();
	}

	public int CurrencyRoundCount
	{
		get => _settingsRepo.CurrencyRoundCount;
		set
		{
			if (value < 0) throw new Exception("Currency round count must be non-negative.");

			_settingsRepo.CurrencyRoundCount = value;
		}
	}
}