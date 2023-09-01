using Domain.Enums;

namespace Application.Public.Interfaces.Rest;

public interface ISettingsWriterService
{
	Task UpdateDefaultCurrency(CurrencyType defaultCurrency);

	Task UpdateCurrencyRoundCount(int currencyRoundCount);
}