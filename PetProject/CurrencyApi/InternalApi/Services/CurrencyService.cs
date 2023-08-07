using Fuse8_ByteMinds.SummerSchool.Application.Contracts;
using Fuse8_ByteMinds.SummerSchool.Application.Extensions;
using Fuse8_ByteMinds.SummerSchool.Domain.Models.Currency;
using Fuse8_ByteMinds.SummerSchool.Domain.Models.Status;
using Fuse8_ByteMinds.SummerSchool.Domain.Options;
using Microsoft.Extensions.Options;

namespace Fuse8_ByteMinds.SummerSchool.InternalApi.Services;

public class CurrencyService : ICurrencyService
{
    private readonly CurrencyServiceOptions _options;
    private readonly string _baseUrl;
    private readonly HttpClient _httpClient;

    public CurrencyService(
        IOptionsSnapshot<CurrencyServiceOptions> options,
        HttpClient httpClient)
    {
        _options = options.Value;
        _baseUrl = _options.BaseUrl;
        _httpClient = httpClient;
        ConfigureRequestHeaders();
    }

    public async Task<Currency> GetDefaultExchangeRate()
    {
        var requestUri = $"{_baseUrl}/latest?currencies={_options.DefaultCurrency}&base_currency={_options.BaseCurrency}";
        var responseMessage = await _httpClient.GetAsync(requestUri);
        var currencyResponse = await responseMessage.EnsureValidAndDeserialize<CurrencyResponse>();
        var currency = currencyResponse.Data[_options.DefaultCurrency];
        return new Currency(currency.Code, RoundValue(currency.Value));
    }

    public async Task<Currency> GetExchangeRateByCode(string currencyCode)
    {
        var requestUri = $"{_baseUrl}/latest?currencies={currencyCode}&base_currency={_options.BaseCurrency}";
        var responseMessage = await _httpClient.GetAsync(requestUri);
        var currencyResponse = await responseMessage.EnsureValidAndDeserialize<CurrencyResponse>();
        var currency = currencyResponse.Data[currencyCode];
        return new Currency(currency.Code, RoundValue(currency.Value));
    }

    public async Task<CurrencyOnDate> GetHistoricalExchangeRateByCode(string currencyCode, string date)
    {
        var requestUri = $"{_baseUrl}/historical?date={date}&currencies={currencyCode}&base_currency={_options.BaseCurrency}";
        var responseMessage = await _httpClient.GetAsync(requestUri);
        var currencyResponse = await responseMessage.EnsureValidAndDeserialize<CurrencyResponse>();
        var currency = currencyResponse.Data[currencyCode];
        return new CurrencyOnDate(date, currency.Code, RoundValue(currency.Value));
    }

    public async Task<Status> GetStatus()
    {
        var requestUri = $"{_baseUrl}/status";
        var responseMessage = await _httpClient.GetAsync(requestUri);
        var statusResponse = await responseMessage.EnsureValidAndDeserialize<StatusResponse>();
        var month = statusResponse.Quotas.Month;
        return new Status(
            DefaultCurrency: _options.DefaultCurrency,
            BaseCurrency: _options.BaseCurrency,
            RequestLimit: month.Total,
            RequestCount: month.Used,
            CurrencyRoundCount: _options.CurrencyRoundCount);
    }

    private void ConfigureRequestHeaders()
        => _httpClient.DefaultRequestHeaders.Add("apikey", _options.ApiKey);

    private decimal RoundValue(decimal value)
            => Math.Round(value, _options.CurrencyRoundCount);
}