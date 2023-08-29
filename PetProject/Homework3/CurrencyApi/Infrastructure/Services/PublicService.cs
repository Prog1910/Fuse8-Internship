using Application.Common.Interfaces;
using Application.Persistence;
using Domain.Enums;

namespace Infrastructure.Services;

public sealed class PublicService : IPublicApi
{
	private readonly ISettingsRepository _repository;

	public PublicService(ISettingsRepository repository)
	{
		_repository = repository;
	}

	public async Task UpdateDefaultCurrencyAsync(CurrencyType defaultCurrency)
	{
		await Task.CompletedTask;
		_repository.UpdateDefaultCurrency(defaultCurrency.ToString());
	}

	public async Task UpdateCurrencyRoundCountAsync(int currencyRoundCount)
	{
		await Task.CompletedTask;
		_repository.UpdateCurrencyRoundCount(currencyRoundCount);
	}
}
