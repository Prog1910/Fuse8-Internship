using Domain.Aggregates;

namespace Application.Internal.Persistence;

public interface ICurrencyRepository
{
	void AddCurrenciesByBaseCode(string baseCurrency, IEnumerable<Currency> currencies, DateTime? date = default);

	IEnumerable<Currency>? GetCurrenciesByBaseCode(string baseCurrency, DateOnly? date = default);

	IEnumerable<CurrenciesOnDateCache>? GetAllCurrenciesOnDates(DateOnly? date = default);

	void UpdateCurrenciesOnDate(CurrenciesOnDateCache currenciesOnDate);
}