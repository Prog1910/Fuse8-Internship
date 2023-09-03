namespace Application.Public.Persistence;

public interface ISettingsRepository
{
	string DefaultCurrencyCode { get; set; }

	int CurrencyRoundCount { get; set; }
}