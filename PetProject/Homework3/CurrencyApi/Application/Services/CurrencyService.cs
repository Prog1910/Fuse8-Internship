using Application.Common.Extensions;
using Application.Services.Common;
using Contracts.Models.Currency;
using Contracts.Models.Status;
using Domain.Common.Options;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class CurrencyService : ICurrencyService
{
	private readonly CurrencyServiceOptions _currencyServiceSettings;
	private readonly HttpClient _httpClient;
	private readonly string _baseUrl;

	public CurrencyService(IOptionsSnapshot<CurrencyServiceOptions> options, HttpClient httpClient)
	{
		_currencyServiceSettings = options.Value;
		_baseUrl = _currencyServiceSettings.BaseUrl;
		_httpClient = httpClient;
		ConfigureRequestHeaders();
	}

	public async Task<Currency> GetCurrencyByDefault()
	{
		var requestUri = $"{_baseUrl}/latest?currencies={_currencyServiceSettings.DefaultCurrency}&base_currency={_currencyServiceSettings.BaseCurrency}";
		var response = await _httpClient.GetAsync(requestUri);
		var currencyResponse = await response.EnsureValidAndDeserialize<CurrencyResponse>();
		var data = currencyResponse.Data[_currencyServiceSettings.DefaultCurrency];
		return new Currency(data.Code, RoundValue(data.Value));
	}

	public async Task<Currency> GetCurrencyByCode(string currencyCode)
	{
		var requestUri = $"{_baseUrl}/latest?currencies={currencyCode}&base_currency={_currencyServiceSettings.BaseCurrency}";
		var responseMessage = await _httpClient.GetAsync(requestUri);
		var currencyResponse = await responseMessage.EnsureValidAndDeserialize<CurrencyResponse>();
		var data = currencyResponse.Data[currencyCode];
		return new Currency(data.Code, RoundValue(data.Value));
	}

	public async Task<CurrencyOnDate> GetHistoricalCurrencyExchangeRate(string currencyCode, string date)
	{
		var requestUri = $"{_baseUrl}/historical?currencies={currencyCode}&date={date}&base_currency={_currencyServiceSettings.BaseCurrency}";
		var response = await _httpClient.GetAsync(requestUri);
		var currencyResponse = await response.EnsureValidAndDeserialize<CurrencyResponse>();
		var data = currencyResponse.Data[currencyCode];
		return new CurrencyOnDate(date, currencyCode, data.Value);
	}

	public async Task<ApiSettings> GetApiSettings()
	{
		var requestUri = $"{_baseUrl}/status";
		var responseMessage = await _httpClient.GetAsync(requestUri);
		var quotaInfo = await responseMessage.EnsureValidAndDeserialize<ApiSettingsResponse>();
		var month = quotaInfo.Quotas.Month;
		return new ApiSettings(
			DefaultCurrency: _currencyServiceSettings.DefaultCurrency,
			BaseCurrency: _currencyServiceSettings.BaseCurrency,
			RequestLimit: month.Total,
			RequestCount: month.Used,
			CurrencyRoundCount: _currencyServiceSettings.CurrencyRoundCount);
	}

	private void ConfigureRequestHeaders()
		=> _httpClient.DefaultRequestHeaders.Add("apikey", _currencyServiceSettings.ApiKey);

	private decimal RoundValue(decimal value)
		=> Math.Round(value, _currencyServiceSettings.CurrencyRoundCount);
}