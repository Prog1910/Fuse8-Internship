using Application.Internal.Interfaces.Rest;
using Application.Internal.Services.Rest.Responses;
using Domain.Aggregates;
using Domain.Enums;
using Domain.Errors;
using Domain.Options;
using Mapster;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;

namespace Application.Internal.Services.Rest;

public sealed class CurrencyService : ICurrencyApi
{
	private readonly string _baseUrl;
	private readonly string _currencyTypes;
	private readonly HttpClient _httpClient;
	private readonly InternalApiOptions _options;

	public CurrencyService(IOptions<InternalApiOptions> options, HttpClient httpClient)
	{
		_options = options.Value;
		_httpClient = httpClient;
		ConfigureRequestHeaders();
		_currencyTypes = CombineCurrencyTypesWithCommas();
		_baseUrl = _options.BaseUrl;
	}

	public async Task<Currency[]> GetAllCurrentCurrenciesAsync(string baseCurrencyCode, CancellationToken cancellationToken)
	{
		string requestUri = $"{_baseUrl}/latest?currencies={_currencyTypes}&base_currency={baseCurrencyCode}";
		HttpResponseMessage response = await _httpClient.GetAsync(requestUri, cancellationToken);
		CurrencyResponse currencyResponse = await EnsureValidAndDeserializeResponse<CurrencyResponse>(response, cancellationToken);

		Currency[] currencies = currencyResponse.Data.Values.ToArray().Adapt<Currency[]>();

		return currencies;
	}

	public async Task<CurrenciesOnDate> GetAllCurrenciesOnDateAsync(string baseCurrencyCode, DateOnly date, CancellationToken cancellationToken)
	{
		string requestUri = $"{_baseUrl}/historical?date={date}&currencies={_currencyTypes}&base_currency={baseCurrencyCode}";
		HttpResponseMessage response = await _httpClient.GetAsync(requestUri, cancellationToken);
		CurrencyResponse currencyResponse = await EnsureValidAndDeserializeResponse<CurrencyResponse>(response, cancellationToken);

		string lastUpdatedAt = currencyResponse.Meta.LastUpdatedAt;
		Currency[] currencies = currencyResponse.Data.Values.Adapt<Currency[]>();
		CurrenciesOnDate currenciesOnDate = new()
		{
			LastUpdatedAt = DateTime.Parse(lastUpdatedAt).Date.ToUniversalTime(),
			Currencies = currencies
		};

		return currenciesOnDate;
	}

	public async Task<Settings> GetSettingsAsync(CancellationToken cancellationToken)
	{
		string requestUri = $"{_baseUrl}/status";
		HttpResponseMessage response = await _httpClient.GetAsync(requestUri, cancellationToken);
		SettingsResponse settingsResponse = await EnsureValidAndDeserializeResponse<SettingsResponse>(response, cancellationToken);

		MonthResponse month = settingsResponse.Quotas.Month;
		Settings settings = new()
		{
			BaseCurrencyCode = _options.BaseCurrencyCode,
			NewRequestsAvailable = month.Total > month.Used
		};

		return settings;
	}

	private static async Task<TResponse> EnsureValidAndDeserializeResponse<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken)
	{
		if (response.IsSuccessStatusCode is false)
		{
			throw response.StatusCode switch
			{
				HttpStatusCode.TooManyRequests => new ApiRequestLimitException(),

				_ => GenerateInternalServerError()
			};
		}

		return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken) ?? throw GenerateInternalServerError();
	}

	private void ConfigureRequestHeaders()
	{
		_httpClient.DefaultRequestHeaders.Add(name: "apikey", _options.ApiKey);
	}

	private static string CombineCurrencyTypesWithCommas()
	{
		return string.Join(separator: ",", Enum.GetValues<CurrencyType>());
	}

	private static Exception GenerateInternalServerError(string message = "An error occurred.", Exception? exception = default)
	{
		return new HttpRequestException(message, exception, HttpStatusCode.InternalServerError);
	}
}
