namespace Application.Shared.Dtos;

public sealed record FullSettingsDto(
	string DefaultCurrency,
	string BaseCurrency,
	bool NewRequestsAvailable,
	int CurrencyRoundCount);