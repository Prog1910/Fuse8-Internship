using Domain.Aggregates.CurrencyAggregate;

namespace Application.Persistence;

public interface ICurrenciesRepository
{
	void AddCurrencies(string baseCurrency, Currency[] currencies, DateTime? date = null);

	Currency[]? GetCurrencies(string baseCurrency, DateOnly? date = null);
}
