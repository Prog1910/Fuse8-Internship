using Shared.Domain.Enums;

namespace Shared.Application.Dtos;

public sealed record SettingsDto(
	CurrencyType BaseCurrencyCode,
	bool NewRequestsAvailable);
