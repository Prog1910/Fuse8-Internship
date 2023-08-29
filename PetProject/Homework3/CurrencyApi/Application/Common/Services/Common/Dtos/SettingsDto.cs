using Domain.Enums;

namespace Application.Common.Services.Common.Dtos;

public sealed record SettingsDto(
	CurrencyType BaseCurrency,
	bool NewRequestsAvailable);
