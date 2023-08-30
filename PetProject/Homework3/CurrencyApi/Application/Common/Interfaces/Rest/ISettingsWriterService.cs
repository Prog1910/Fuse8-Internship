using Domain.Enums;

namespace Application.Common.Interfaces.Rest;

public interface ISettingsWriterService
{
	Task UpdateDefaultCurrency(CurrencyType defaultCurrency);

	Task UpdateCurrencyRoundCount(int currencyRoundCount);
}