using Application.Common.Errors;
using Application.Common.Interfaces.Rest;
using Application.Common.Services.Rest.Common.Responses;
using Domain.Aggregates.CurrenciesOnDateAggregate;
using Domain.Aggregates.CurrencyAggregate;
using Domain.Aggregates.SettingsAggregate;
using Domain.Enums;
using Domain.Options;
using MapsterMapper;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;

namespace Application.Common.Services.Rest;

public sealed class CurrencyService : ICurrencyApi
{
    private readonly string _baseUrl;
    private readonly string _currencyTypes;
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly InternalApiOptions _options;

    public CurrencyService(IOptionsSnapshot<InternalApiOptions> options, HttpClient httpClient, IMapper mapper)
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
        var currenciesOnDate = new CurrenciesOnDate
        {
            LastUpdatedAt = DateTime.Parse(lastUpdatedAt).Date.ToUniversalTime(),
            Currencies = currencies
        };

        return currenciesOnDate;
    }

    public async Task<Settings> GetSettingsAsync(CancellationToken cancellationToken)
    {
        var requestUri = $"{_baseUrl}/status";
        var response = await _httpClient.GetAsync(requestUri, cancellationToken);
        EnsureValidResponse(response);

        var settingsResponse = await response.Content.ReadFromJsonAsync<SettingsResponse>(cancellationToken: cancellationToken);
        var month = settingsResponse?.Quotas.Month ?? throw GenerateExceptionWithInternalServerError("Month in SettingsResponse not found.");
        var settings = new Settings
        {
            BaseCurrency = _options.BaseCurrency,
            NewRequestsAvailable = month.Total > month.Used
        };

        return settings;
    }

    private void ConfigureRequestHeaders()
    {
        _httpClient.DefaultRequestHeaders.Add(name: "apikey", _options.ApiKey);
    }

    private static string CombineCurrencyTypesWithCommas()
    {
        return string.Join(separator: ",", Enum.GetValues<CurrencyType>());
    }

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
            message,
            exception,
            HttpStatusCode.InternalServerError);
    }
}