using Application.Common.Errors;
using Application.Common.Interfaces.Rest;
using Application.Common.Services.Rest.Common.Dtos;
using Application.Persistence;
using Domain.Enums;
using Domain.Options;
using MapsterMapper;
using Microsoft.Extensions.Options;

namespace Application.Common.Services.Rest;

public sealed class CachedCurrencyService : ICachedCurrencyApi
{
	private readonly ICurrencyApi _currencyApi;
	private readonly IMapper _mapper;
	private readonly InternalApiOptions _options;
	private readonly ICurrenciesRepository _repository;

	public CachedCurrencyService(IOptionsSnapshot<InternalApiOptions> options, ICurrenciesRepository repository, ICurrencyApi currencyApi, IMapper mapper)
	{
		_options = options.Value;
		_repository = repository;
		_currencyApi = currencyApi;
		_mapper = mapper;
	}

	public async Task<CurrencyDto> GetCurrentCurrencyAsync(CurrencyType defaultCurrency, CancellationToken cancellationToken)
	{
		var currencies = _repository.GetCurrencies(_options.BaseCurrency);
		if (currencies is null)
		{
			currencies = await _currencyApi.GetAllCurrentCurrenciesAsync(_options.BaseCurrency, cancellationToken);
			_repository.AddCurrencies(_options.BaseCurrency, currencies);
		}
		var currency = currencies.SingleOrDefault(c => c.Code.Equals(defaultCurrency.ToString())) ?? throw new CurrencyNotFoundException();

		return _mapper.Map<CurrencyDto>(currency);
	}

	public async Task<CurrencyDto> GetCurrencyOnDateAsync(CurrencyType defaultCurrency, DateOnly date, CancellationToken cancellationToken)
	{
		// ? Логично ли
		if (date.Equals(DateOnly.FromDateTime(DateTime.UtcNow)))
			return await GetCurrentCurrencyAsync(defaultCurrency, cancellationToken);

		var currencies = _repository.GetCurrencies(_options.BaseCurrency, date);
		if (currencies is null)
		{
			var currenciesOnDate = await _currencyApi.GetAllCurrenciesOnDateAsync(_options.BaseCurrency, date, cancellationToken);
			var lastUpdateAt = currenciesOnDate.LastUpdatedAt.Date.ToUniversalTime();
			currencies = currenciesOnDate.Currencies;
			_repository.AddCurrencies(_options.BaseCurrency, currencies, lastUpdateAt);
		}
		var currency = currencies.SingleOrDefault(c => c.Code.Equals(defaultCurrency.ToString())) ?? throw new CurrencyNotFoundException();

		return _mapper.Map<CurrencyDto>(currency);
	}

	public async Task<CurrencyDto> GetCurrentFavoriteCurrencyAsync(CurrencyType defaultCurrency, CurrencyType baseCurrency, CancellationToken cancellationToken)
	{
		var defaultCurrencyDto = await GetCurrentCurrencyAsync(defaultCurrency, cancellationToken);
		if (_options.BaseCurrency.Equals(baseCurrency.ToString())) return defaultCurrencyDto;

		var baseCurrencyDto = await GetCurrentCurrencyAsync(baseCurrency, cancellationToken);
		var newValue = defaultCurrencyDto.Value / baseCurrencyDto.Value;
		var resultCurrencyDto = new CurrencyDto(defaultCurrency, newValue);

		return resultCurrencyDto;
	}

	public async Task<CurrencyDto> GetFavoriteCurrencyOnDateAsync(CurrencyType defaultCurrency, CurrencyType baseCurrency, DateOnly date, CancellationToken cancellationToken)
	{
		var defaultCurrencyDto = await GetCurrencyOnDateAsync(defaultCurrency, date, cancellationToken);
		if (_options.BaseCurrency.Equals(baseCurrency.ToString())) return defaultCurrencyDto;

		var baseCurrencyDto = await GetCurrencyOnDateAsync(baseCurrency, date, cancellationToken);
		var newValue = defaultCurrencyDto.Value / baseCurrencyDto.Value;
		var resultCurrencyDto = new CurrencyDto(defaultCurrency, newValue);

		return resultCurrencyDto;
	}

	public async Task<SettingsDto> GetSettingsAsync(CancellationToken cancellationToken)
	{
		var settings = await _currencyApi.GetSettingsAsync(cancellationToken);

		return _mapper.Map<SettingsDto>(settings);
	}
}