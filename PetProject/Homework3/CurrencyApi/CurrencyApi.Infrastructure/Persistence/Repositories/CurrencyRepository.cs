using CurrencyApi.Application.Persistence;
using CurrencyApi.Domain.Aggregates.CurrencyAggregate;
using System.Text.Json;

namespace CurrencyApi.Infrastructure.Persistence.Repositories;

public sealed class CurrencyRepository : ICurrencyRepository
{
	private const string CachedCurrenciesDirectoryName = "CachedCurrencies";
	private const string CachedCurrenciesOnDateDirectoryName = "CachedCurrenciesOnDate";
	private const string JsonFileNameExtension = ".json";

	private readonly TimeSpan _expirationTime;
	private readonly string _cacheDirectoryPath;
	private readonly string _cacheOnDateDirectoryPath;

	public CurrencyRepository()
	{
		_cacheDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), CachedCurrenciesDirectoryName);
		_cacheOnDateDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), CachedCurrenciesOnDateDirectoryName);
		_expirationTime = TimeSpan.FromHours(2);
	}

	public void AddCurrentCurrencies(string baseCurrency, Currency[] currencies)
	{
		var formattedDate = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss");
		var cacheFilePath = Path.Combine(_cacheDirectoryPath, $"{baseCurrency}_{formattedDate}{JsonFileNameExtension}");
		Directory.CreateDirectory(_cacheDirectoryPath);

		using FileStream fileStream = File.Open(cacheFilePath, FileMode.OpenOrCreate);
		JsonSerializer.Serialize(
			utf8Json: fileStream,
			value: currencies,
			options: new JsonSerializerOptions { WriteIndented = true });
	}

	public Currency[]? GetCurrentCurrencies(string baseCurrency)
	{
		if (Directory.Exists(_cacheDirectoryPath))
		{
			var cacheFiles = Directory.GetFiles(_cacheDirectoryPath, $"{baseCurrency}_*{JsonFileNameExtension}")
				.OrderByDescending(File.GetLastWriteTimeUtc)
				.FirstOrDefault();

			if (cacheFiles is not null
				&& IsCacheExpired(File.GetLastWriteTimeUtc(cacheFiles), _expirationTime) == false)
			{
				var currencies = JsonSerializer.Deserialize<Currency[]>(File.ReadAllText(cacheFiles));

				return currencies;
			}
		}

		return null;
	}

	public void AddCurrenciesOnDate(string baseCurrency, DateTime date, Currency[] currencies)
	{
		var formattedDate = date.ToString("yyyy-MM-dd_HH-mm-ss");
		var cacheFilePath = Path.Combine(_cacheOnDateDirectoryPath, $"{baseCurrency}_{formattedDate}{JsonFileNameExtension}");
		Directory.CreateDirectory(_cacheOnDateDirectoryPath);

		using FileStream fileStream = File.Open(cacheFilePath, FileMode.OpenOrCreate);
		JsonSerializer.Serialize(
			utf8Json: fileStream,
			value: currencies,
			options: new JsonSerializerOptions { WriteIndented = true });
	}

	public Currency[]? GetCurrenciesOnDate(string baseCurrency, DateOnly date)
	{
		var formattedDate = date.ToString("yyyy-MM-dd");

		if (Directory.Exists(_cacheOnDateDirectoryPath))
		{
			var cacheFiles = Directory.GetFiles(_cacheOnDateDirectoryPath, $"{baseCurrency}_{formattedDate}*{JsonFileNameExtension}")
				.OrderByDescending(File.GetLastWriteTimeUtc)
				.FirstOrDefault();

			if (cacheFiles is not null)
			{
				var cacheTime = File.GetLastWriteTimeUtc(cacheFiles);
				var currencies = JsonSerializer.Deserialize<Currency[]>(File.ReadAllText(cacheFiles));

				return currencies;
			}
		}

		return null;
	}

	private static bool IsCacheExpired(DateTime cacheTime, TimeSpan exprirationTime)
		=> DateTime.UtcNow.Subtract(cacheTime) >= exprirationTime;
}