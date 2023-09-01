namespace Application.Public.Interfaces.Rest;

public interface ISettingsReaderService
{
	string DefaultCurrency { get; }

	int CurrencyRoundCount { get; }
}