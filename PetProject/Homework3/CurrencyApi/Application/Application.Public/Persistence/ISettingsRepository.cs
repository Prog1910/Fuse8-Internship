namespace Application.Public.Persistence;

public interface ISettingsRepository
{
	string? DefaultCurrency { get; }

	int? CurrencyRoundCount { get; }

	void UpdateDefaultCurrency(string defaultCurrency);

	void UpdateCurrencyRoundCount(int currencyRoundCount);
}