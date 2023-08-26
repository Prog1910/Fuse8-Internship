using CurrencyApi.Application.Common.Errors;
using CurrencyApi.Application.Common.Interfaces;
using CurrencyApi.Application.Common.Services.Common.Responses;
using CurrencyApi.Domain.Aggregates.CurrenciesOnDateAggregate;
using CurrencyApi.Domain.Aggregates.CurrencyAggregate;
using CurrencyApi.Domain.Aggregates.SettingsAggregate;
using CurrencyApi.Domain.Enums;
using CurrencyApi.Domain.Options;
using MapsterMapper;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;

namespace CurrencyApi.Application.Common.Services;

public sealed class CurrencyService : ICurrencyApi
{
	private readonly CurrencyApiOptions _options;
	private readonly HttpClient _httpClient;
	private readonly IMapper _mapper;
	private readonly string _currencyTypes;
	private readonly string _baseUrl;

	public CurrencyService(IOptionsSnapshot<CurrencyApiOptions> options, HttpClient httpClient, IMapper mapper)
	{
		_options = options.Value;
		_httpClient = httpClient;
		_mapper = mapper;
		ConfigureRequestHeaders();
		_currencyTypes = CombineCurrencyTypesWithCommas();
		_baseUrl = _options.BaseUrl;
	}

	public async Task<Currency[]> GetAllCurrentCurrenciesAsync(string baseCurrency, CancellationToken cancellationToken)
	{
		var requestUri = $"{_baseUrl}/latest?currencies={_currencyTypes}&base_currency={baseCurrency}";
		var response = await _httpClient.GetAsync(requestUri, cancellationToken);
		EnsureValidResponse(response);

		var currencyResponse = await response.Content.ReadFromJsonAsync<CurrencyResponse>(cancellationToken: cancellationToken);
		var currencies = _mapper.Map<Currency[]>(currencyResponse?.Data.Values.ToArray() ?? throw new CurrencyNotFoundException());

		return currencies;
	}

	public async Task<CurrenciesOnDate> GetAllCurrenciesOnDateAsync(string baseCurrency, DateOnly date, CancellationToken cancellationToken)
	{
		var requestUri = $"{_baseUrl}/historical?date={date}&currencies={_currencyTypes}&base_currency={baseCurrency}";
		var response = await _httpClient.GetAsync(requestUri, cancellationToken);
		EnsureValidResponse(response);

		var currencyResponse = await response.Content.ReadFromJsonAsync<CurrencyResponse>(cancellationToken: cancellationToken);
		var lastUpdatedAt = currencyResponse?.Meta.LastUpdatedAt ?? throw GenerateExceptionWithInternalServerError("Meta in CurrencyResponse not found.");
		var currencies = _mapper.Map<Currency[]>(currencyResponse?.Data.Values.ToArray() ?? throw GenerateExceptionWithInternalServerError("Data in CurrencyResponse not found."));
		var currenciesOnDate = new CurrenciesOnDate(DateTime.Parse(lastUpdatedAt), currencies);

		return currenciesOnDate;
	}

	public async Task<Settings> GetSettingsAsync(CancellationToken cancellationToken)
	{
		var requestUri = $"{_baseUrl}/status";
		var response = await _httpClient.GetAsync(requestUri, cancellationToken);
		EnsureValidResponse(response);

		var settingsResponse = await response.Content.ReadFromJsonAsync<SettingsResponse>(cancellationToken: cancellationToken);
		var month = settingsResponse?.Quotas.Month ?? throw GenerateExceptionWithInternalServerError("Month in SettingsResponse not found.");
		var settings = new Settings(
			DefaultCurrency: _options.DefaultCurrency,
			BaseCurrency: _options.BaseCurrency,
			NewRequestsAvailable: month.Total > month.Used,
			CurrencyRoundCount: _options.CurrencyRoundCount);

		return settings;
	}

	private void ConfigureRequestHeaders()
		=> _httpClient.DefaultRequestHeaders.Add("apikey", _options.ApiKey);

	private static string CombineCurrencyTypesWithCommas()
		=> string.Join(",", Enum.GetValues<CurrencyType>());

	public static void EnsureValidResponse(HttpResponseMessage response)
	{
		if (response.IsSuccessStatusCode == false)
		{
			throw response.StatusCode switch
			{
				HttpStatusCode.TooManyRequests => new ApiRequestLimitException(),
				_ => GenerateExceptionWithInternalServerError()
			};
		}
	}

	public static Exception GenerateExceptionWithInternalServerError(string message = "An error occurred.", Exception? exception = null)
	{
		return new HttpRequestException(
			message: message,
			inner: exception,
			statusCode: HttpStatusCode.InternalServerError);
	}
}