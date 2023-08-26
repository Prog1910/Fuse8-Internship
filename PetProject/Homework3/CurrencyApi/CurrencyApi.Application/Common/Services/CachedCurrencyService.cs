using CurrencyApi.Application.Common.Interfaces;
using CurrencyApi.Application.Common.Services.Common.Dtos;
using CurrencyApi.Application.Persistence;
using CurrencyApi.Domain.Enums;
using CurrencyApi.Domain.Options;
using MapsterMapper;
using Microsoft.Extensions.Options;

namespace CurrencyApi.Application.Common.Services;

public sealed class CachedCurrencyService : ICachedCurrencyApi
{
	private readonly CurrencyApiOptions _options;
	private readonly ICurrencyRepository _repository;
	private readonly ICurrencyApi _currencyApi;
	private readonly IMapper _mapper;

	public CachedCurrencyService(IOptionsSnapshot<CurrencyApiOptions> options, ICurrencyRepository repository, ICurrencyApi currencyApi, IMapper mapper)
	{
		_options = options.Value;
		_repository = repository;
		_currencyApi = currencyApi;
		_mapper = mapper;
	}

	public async Task<CurrencyDto> GetCurrentCurrencyAsync(CurrencyType currencyType, CancellationToken cancellationToken)
	{
		if (currencyType is 0) currencyType = Enum.Parse<CurrencyType>(_options.DefaultCurrency);
		var currencies = _repository.GetCurrentCurrencies(_options.BaseCurrency);
		if (currencies is null)
		{
			currencies = await _currencyApi.GetAllCurrentCurrenciesAsync(_options.BaseCurrency, cancellationToken);
			_repository.AddCurrentCurrencies(_options.BaseCurrency, currencies);
		}
		var currency = currencies.Single(c => c.Code.Equals(currencyType.ToString()));

		return _mapper.Map<CurrencyDto>(currency);
	}

	public async Task<CurrencyDto> GetCurrencyOnDateAsync(CurrencyType currencyType, DateOnly date, CancellationToken cancellationToken)
	{
		if (currencyType is 0) currencyType = Enum.Parse<CurrencyType>(_options.DefaultCurrency);
		var currencies = _repository.GetCurrenciesOnDate(_options.BaseCurrency, date);
		if (currencies is null)
		{
			var currenciesOnDate = await _currencyApi.GetAllCurrenciesOnDateAsync(_options.BaseCurrency, date, cancellationToken);
			var lastUpdateAt = currenciesOnDate.LastUpdatedAt;
			currencies = currenciesOnDate.Currencies;
			_repository.AddCurrenciesOnDate(_options.BaseCurrency, lastUpdateAt, currencies);
		}
		var currency = currencies.Single(c => c.Code.Equals(currencyType.ToString()));

		return _mapper.Map<CurrencyDto>(currency);
	}

	public async Task<SettingsDto> GetSettingsAsync(CancellationToken cancellationToken)
	{
		var settings = await _currencyApi.GetSettingsAsync(cancellationToken);

		return _mapper.Map<SettingsDto>(settings);
	}
}