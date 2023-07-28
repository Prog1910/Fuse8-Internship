﻿using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Currency;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Status;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers;

/// <summary>
/// Methods for handling exchange rate conversion
/// </summary>
[Route("currencyapi/")]
public class CurrencyController : Controller
{
	private readonly CurrencyApiSettings _currencyServiceSettings;
	private readonly HttpClient _httpClient;

	public CurrencyController(IOptions<CurrencyApiSettings> options, HttpClient httpClient)
	{
		_currencyServiceSettings = options.Value;
		_httpClient = httpClient;
	}

	#region GET /currency
	/// <summary>
	/// Latest Сurrency Exchange Data (default currency RUB, default base currency USD)
	/// </summary>
	/// <response code="200">
	/// Returns if it was possible to get the default currency data
	/// </response>
	/// <response code="500">
	/// Returns if the default currency data could not be retrieved
	/// </response>
	[HttpGet("currency")]
	public async Task<CurrencyDataDto> GetCurrencyExchangeRate()
	{
		_httpClient.DefaultRequestHeaders.Add("apikey", _currencyServiceSettings.ApiKey);
		var requestUri = $"https://api.currencyapi.com/v3/latest?currencies={_currencyServiceSettings.DefaultCurrency}&base_currency={_currencyServiceSettings.BaseCurrency}";
		var responseMessage = await _httpClient.GetAsync(requestUri);
		if (responseMessage.IsSuccessStatusCode)
		{
			var responseContent = await responseMessage.Content.ReadAsStringAsync();
			var currencyResponse = JsonSerializer.Deserialize<CurrencyResponseDto>(responseContent);
			if (currencyResponse is not null)
			{
				var currencyData = currencyResponse.CurrenciesData[_currencyServiceSettings.DefaultCurrency];
				return new(currencyData.Code, Math.Round(currencyData.Value, _currencyServiceSettings.CurrencyRoundCount));
			}
		}
		throw new HttpRequestException("Failed to get default currency data", null, HttpStatusCode.InternalServerError);
	}
	#endregion

	#region GET /currency/{currencyCode}
	/// <summary>
	/// Latest Сurrency Exchange Data (default base currency USD)
	/// </summary>
	/// <param name="currencyCode">Сurrency the current rate of which will be returned</param>
	/// <response code="200">
	/// Returns if data was received successfully currency passed as a parameter
	/// </response>
	/// <response code="500">
	/// Returns if the data could not be retrieved currency passed as a parameter
	/// </response>
	[HttpGet("/currency/{currencyCode}")]
	public async Task<CurrencyDataDto> GetCurrencyExchangeRateByCode(string currencyCode)
	{
		_httpClient.DefaultRequestHeaders.Add("apikey", _currencyServiceSettings.ApiKey);
		var validCurrencyCode = currencyCode.ToUpper();
		var requestUri = $"https://api.currencyapi.com/v3/latest?currencies={validCurrencyCode}&base_currency={_currencyServiceSettings.BaseCurrency}";
		var responseMessage = await _httpClient.GetAsync(requestUri);
		if (responseMessage.IsSuccessStatusCode)
		{
			var responseContent = await responseMessage.Content.ReadAsStringAsync();
			var currencyResponse = JsonSerializer.Deserialize<CurrencyResponseDto>(responseContent);
			if (currencyResponse is not null)
			{
				var currencyData = currencyResponse.CurrenciesData[validCurrencyCode];
				return new(currencyData.Code, Math.Round(currencyData.Value, _currencyServiceSettings.CurrencyRoundCount));
			}
		}
		throw new HttpRequestException("Failed to get default currency data", null, HttpStatusCode.InternalServerError);
	}
	#endregion

	#region GET /currency/{currencyCode}/{date}
	/*
	 * GET /currency/{currencyCode}/{date}
	{date} должна иметь формат yyyy-MM-dd
	Должен вызывать метод внешнего API https://api.currencyapi.com/v3/historical?currencies={currencyCode}&date={date}&base_currency=USD согласно документации
	Должен возвращать курс валюты, переданной в качестве параметра, на определенную дата
	Базовую валюту USD необходимо брать из конфигурации приложения.
	Курс должен округляться до двух знаков после зяпятой. Количество знаков следует брать из конфигурации приложения.
	Должен возвращать JSON вида (дата должна иметь формат yyyy-MM-dd)
	*/
	//[HttpGet("/currency/{currencyCode}")]


	#endregion

	#region GET /settings
	/// <summary>
	/// The status endpoint returns information about your current quota
	/// </summary>
	/// <response code="200">
	/// Returns if it was possible to get the information about your current quota
	/// </response>
	/// <response code="500">
	/// Returns if the information about your current quota could not be retrieved
	/// </response>
	[HttpGet("settings")]
	public async Task<CurrentStatusDto> GetApiSettings()
	{
		_httpClient.DefaultRequestHeaders.Add("apikey", _currencyServiceSettings.ApiKey);
		var requestUri = "https://api.currencyapi.com/v3/status";
		var responseMessage = await _httpClient.GetAsync(requestUri);
		if (responseMessage.IsSuccessStatusCode)
		{
			var responseContent = await responseMessage.Content.ReadAsStringAsync();
			var apiStatus = JsonSerializer.Deserialize<ApiStatusDto>(responseContent);
			if (apiStatus is not null)
			{
				var month = apiStatus.Quotas.Month;
				return new(_currencyServiceSettings.DefaultCurrency,
					_currencyServiceSettings.BaseCurrency,
					month.Total,
					month.Used,
					_currencyServiceSettings.CurrencyRoundCount);
			}
		}
		throw new HttpRequestException("Failed to get current settings", null, HttpStatusCode.InternalServerError);
	}
	#endregion
}
