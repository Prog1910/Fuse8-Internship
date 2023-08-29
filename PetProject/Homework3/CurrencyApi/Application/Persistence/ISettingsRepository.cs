namespace Application.Persistence;

public interface ISettingsRepository
{
	void UpdateDefaultCurrency(string defaultCurrency);

	void UpdateCurrencyRoundCount(int currencyRoundCount);
}
