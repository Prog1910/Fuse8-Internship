using Domain.Enums;

namespace Application.Shared.Dtos;

public sealed record SettingsDto(
	CurrencyType BaseCurrencyCode,
	bool NewRequestsAvailable);