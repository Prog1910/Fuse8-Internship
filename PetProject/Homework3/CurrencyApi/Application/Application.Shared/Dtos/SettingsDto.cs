using Domain.Enums;

namespace Application.Shared.Dtos;

public sealed record SettingsDto(
	CurrencyType BaseCurrency,
	bool NewRequestsAvailable);