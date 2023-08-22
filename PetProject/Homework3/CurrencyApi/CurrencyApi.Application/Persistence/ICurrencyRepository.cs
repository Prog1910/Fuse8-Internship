using CurrencyApi.Domain.Aggregates.CurrencyAggregate;

namespace CurrencyApi.Application.Persistence;

public interface ICurrencyRepository
{
	void AddCurrentCurrencies(string baseCurrency, Currency[] currencies);

	void AddCurrenciesOnDate(string baseCurrency, DateTime date, Currency[] currencies);

	Currency[]? GetCurrentCurrencies(string baseCurrency);

	Currency[]? GetCurrenciesOnDate(string baseCurrency, DateOnly date);
}