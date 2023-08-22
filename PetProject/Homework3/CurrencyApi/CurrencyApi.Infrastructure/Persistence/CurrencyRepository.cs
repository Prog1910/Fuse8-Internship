using CurrencyApi.Application.Persistence;
using CurrencyApi.Domain.Aggregates.CurrencyAggregate;
using System.Text.Json;

namespace CurrencyApi.Infrastructure.Persistence;

public sealed class CurrencyRepository : ICurrencyRepository
{
	private const string CachedCurrenciesDirectoryName = "CachedCurrencies";
	private const string JsonFileNameExtension = ".json";

	private readonly TimeSpan _expirationTime;
	private readonly string _cacheDirectoryPath;

	public CurrencyRepository()
	{
		_cacheDirectoryPath = Path.Combine(@"D:\Projects\Code\PetProject\Homework3\CurrencyApi", CachedCurrenciesDirectoryName);
		_expirationTime = TimeSpan.FromHours(2);
	}

	public void AddCurrentCurrencies(string baseCurrency, Currency[] currencies)
	{
		// Prepare directory and file for currencies
		var formattedDate = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss");
		var cacheFilePath = Path.Combine(_cacheDirectoryPath, $"{baseCurrency}_{formattedDate}{JsonFileNameExtension}");
		Directory.CreateDirectory(_cacheDirectoryPath);

		// Write currencies to file
		using FileStream fileStream = File.Open(cacheFilePath, FileMode.OpenOrCreate);
		JsonSerializer.Serialize(
			utf8Json: fileStream,
			value: currencies,
			options: new JsonSerializerOptions { WriteIndented = true });
	}

	public Currency[]? GetCurrentCurrencies(string baseCurrency)
	{
		// Check directory and file for currencies
		if (Directory.Exists(_cacheDirectoryPath))
		{
			var cacheFilesOnDate = Directory.GetFiles(_cacheDirectoryPath, $"{baseCurrency}_*{JsonFileNameExtension}")
				.OrderByDescending(File.GetLastWriteTimeUtc)
				.FirstOrDefault();

			if (cacheFilesOnDate is not null
				&& IsCacheExpired(File.GetLastWriteTimeUtc(cacheFilesOnDate), _expirationTime) == false)
			{
				// Get current currencies
				var currencies = JsonSerializer.Deserialize<Currency[]>(File.ReadAllText(cacheFilesOnDate));

				return currencies;
			}
		}

		return null;
	}

	public void AddCurrenciesOnDate(string baseCurrency, DateTime date, Currency[] currencies)
	{
		// Prepare directory and file for currencies
		var formattedDate = date.ToString("yyyy-MM-dd_HH-mm-ss");
		var cacheFilePath = Path.Combine(_cacheDirectoryPath, $"{baseCurrency}_{formattedDate}{JsonFileNameExtension}");
		Directory.CreateDirectory(_cacheDirectoryPath);

		// Write currencies to file
		using FileStream fileStream = File.Open(cacheFilePath, FileMode.OpenOrCreate);
		JsonSerializer.Serialize(
			utf8Json: fileStream,
			value: currencies,
			options: new JsonSerializerOptions { WriteIndented = true });
	}

	public Currency[]? GetCurrenciesOnDate(string baseCurrency, DateOnly date)
	{
		// Check directory and file for currencies
		var formattedDate = date.ToString("yyyy-MM-dd");

		if (Directory.Exists(_cacheDirectoryPath))
		{
			var cacheFilesOnDate = Directory.GetFiles(_cacheDirectoryPath, $"{baseCurrency}_{formattedDate}_*{JsonFileNameExtension}")
				.OrderByDescending(File.GetLastWriteTimeUtc)
				.FirstOrDefault();

			if (cacheFilesOnDate is not null)
			{
				// Get current currencies
				var cacheTime = File.GetLastWriteTimeUtc(cacheFilesOnDate);
				var currencies = JsonSerializer.Deserialize<Currency[]>(File.ReadAllText(cacheFilesOnDate));

				return currencies;
			}
		}

		return null;
	}

	private static bool IsCacheExpired(DateTime cacheTime, TimeSpan exprirationTime)
		=> DateTime.UtcNow.Subtract(cacheTime) >= exprirationTime;
}