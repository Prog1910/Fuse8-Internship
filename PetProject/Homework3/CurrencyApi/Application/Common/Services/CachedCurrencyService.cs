using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Common.Services.Common.Dtos;
using Application.Persistence;
using Domain.Enums;
using Domain.Options;
using MapsterMapper;
using Microsoft.Extensions.Options;

namespace Application.Common.Services;

public sealed class CachedCurrencyService : ICachedCurrencyApi
{
	private readonly InternalApiOptions _options;
	private readonly ICurrencyRepository _repository;
	private readonly ICurrencyApi _currencyApi;
	private readonly IMapper _mapper;

	public CachedCurrencyService(IOptionsSnapshot<InternalApiOptions> options, ICurrencyRepository repository, ICurrencyApi currencyApi, IMapper mapper)
	{
		_options = options.Value;
		_repository = repository;
		_currencyApi = currencyApi;
		_mapper = mapper;
	}

	public async Task<CurrencyDto> GetCurrentCurrencyAsync(CurrencyType currencyType, CancellationToken cancellationToken)
	{
		if (currencyType is 0) currencyType = Enum.Parse<CurrencyType>(_options.DefaultCurrency);
		var currencies = _repository.GetCurrencies(_options.BaseCurrency);
		if (currencies is null)
		{
			currencies = await _currencyApi.GetAllCurrentCurrenciesAsync(_options.BaseCurrency, cancellationToken);
			_repository.AddCurrencies(_options.BaseCurrency, currencies);
		}
		var currency = currencies.SingleOrDefault(c => c.Code.Equals(currencyType.ToString())) ?? throw new CurrencyNotFoundException();

		return _mapper.Map<CurrencyDto>(currency);
	}

	public async Task<CurrencyDto> GetCurrencyOnDateAsync(CurrencyType currencyType, DateOnly date, CancellationToken cancellationToken)
	{
		if (currencyType is 0) currencyType = Enum.Parse<CurrencyType>(_options.DefaultCurrency);
		var currencies = _repository.GetCurrencies(_options.BaseCurrency, date);
		if (currencies is null)
		{
			var currenciesOnDate = await _currencyApi.GetAllCurrenciesOnDateAsync(_options.BaseCurrency, date, cancellationToken);
			var lastUpdateAt = currenciesOnDate.LastUpdatedAt.Date.ToUniversalTime();
			currencies = currenciesOnDate.Currencies;
			_repository.AddCurrencies(_options.BaseCurrency, currencies, lastUpdateAt);
		}
		var currency = currencies.SingleOrDefault(c => c.Code.Equals(currencyType.ToString())) ?? throw new CurrencyNotFoundException();

		return _mapper.Map<CurrencyDto>(currency);
	}

	public async Task<SettingsDto> GetSettingsAsync(CancellationToken cancellationToken)
	{
		var settings = await _currencyApi.GetSettingsAsync(cancellationToken);

		return _mapper.Map<SettingsDto>(settings);
	}
}
