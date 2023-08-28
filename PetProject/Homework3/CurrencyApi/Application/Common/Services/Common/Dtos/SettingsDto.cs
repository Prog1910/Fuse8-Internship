using Domain.Enums;

namespace Application.Common.Services.Common.Dtos;

public sealed record SettingsDto(
	CurrencyType DefaultCurrency,
	CurrencyType BaseCurrency,
	bool NewRequestsAvailable,
	int CurrencyRoundCount);