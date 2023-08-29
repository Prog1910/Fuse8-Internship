using Domain.Enums;

namespace Application.Common.Interfaces;

public interface IPublicApi
{
	public Task UpdateDefaultCurrencyAsync(CurrencyType defaultCurrency);

	public Task UpdateCurrencyRoundCountAsync(int currencyRoundCount);
}
