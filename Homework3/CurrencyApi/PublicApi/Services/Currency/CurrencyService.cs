using Fuse8_ByteMinds.SummerSchool.PublicApi.Extensions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Currency;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Status;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Settings;
using Microsoft.Extensions.Options;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services.Currency;

public class CurrencyService : ICurrencyService
{
	private readonly CurrencyApiSettings _currencyServiceSettings;
	private readonly HttpClient _httpClient;
	private readonly string _baseUrl;

	public CurrencyService(IOptionsSnapshot<CurrencyApiSettings> options, HttpClient httpClient)
	{
		_currencyServiceSettings = options.Value;
		_baseUrl = _currencyServiceSettings.BaseUrl;
		_httpClient = httpClient;
		ConfigureRequestHeaders();
	}

	public async Task<CurrencyDataDto> GetCurrencyExchangeRate()
	{
		var requestUri = $"{_baseUrl}/latest?currencies={_currencyServiceSettings.DefaultCurrency}&base_currency={_currencyServiceSettings.BaseCurrency}";
		var response = await _httpClient.GetAsync(requestUri);
		var currencyResponse = await response.EnsureValidAndDeserialize<CurrencyResponse>();
		var data = currencyResponse.Data[_currencyServiceSettings.DefaultCurrency];
		return new CurrencyDataDto(data.Code, RoundValue(data.Value));
	}

	public async Task<CurrencyDataDto> GetCurrencyExchangeRateByCode(string currencyCode)
	{
		var requestUri = $"{_baseUrl}/latest?currencies={currencyCode}&base_currency={_currencyServiceSettings.BaseCurrency}";
		var responseMessage = await _httpClient.GetAsync(requestUri);
		var currencyResponse = await responseMessage.EnsureValidAndDeserialize<CurrencyResponse>();
		var data = currencyResponse.Data[currencyCode];
		return new CurrencyDataDto(data.Code, RoundValue(data.Value));
	}

	public async Task<HistoricalCurrencyDataDto> GetHistoricalCurrencyExchangeRate(string currencyCode, string date)
	{
		var requestUri = $"{_baseUrl}/historical?currencies={currencyCode}&date={date}&base_currency={_currencyServiceSettings.BaseCurrency}";
		var response = await _httpClient.GetAsync(requestUri);
		var currencyResponse = await response.EnsureValidAndDeserialize<CurrencyResponse>();
		var data = currencyResponse.Data[currencyCode];
		return new HistoricalCurrencyDataDto(date, data.Code, RoundValue(data.Value));
	}

	public async Task<CurrentStatusDto> GetApiSettings()
	{
		var requestUri = $"{_baseUrl}/status";
		var responseMessage = await _httpClient.GetAsync(requestUri);
		var quotaInfo = await responseMessage.EnsureValidAndDeserialize<StatusResponse>();
		var month = quotaInfo.Quotas.Month;
		return new CurrentStatusDto(
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