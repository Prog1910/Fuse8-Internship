namespace Application.Shared.Dtos;

public sealed record FullSettingsDto(
	string DefaultCurrencyCode,
	string BaseCurrencyCode,
	bool NewRequestsAvailable,
	int CurrencyRoundCount);