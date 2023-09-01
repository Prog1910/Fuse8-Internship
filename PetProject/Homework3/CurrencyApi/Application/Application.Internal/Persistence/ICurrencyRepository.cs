using Domain.Aggregates;

namespace Application.Internal.Persistence;

public interface ICurrenciesRepository
{
	void AddCurrencies(string baseCurrency, Currency[] currencies, DateTime? date = null);

	Currency[]? GetCurrencies(string baseCurrency, DateOnly? date = null);
}