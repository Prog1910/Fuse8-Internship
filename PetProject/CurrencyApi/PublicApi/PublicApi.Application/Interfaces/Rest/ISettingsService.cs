using Shared.Domain.Enums;

namespace PublicApi.Application.Interfaces.Rest;

public interface ISettingsService
{
	CurrencyType DefaultCurrencyCode { get; }

	int CurrencyRoundCount { get; }

	Task UpdateDefaultCurrencyCodeAsync(CurrencyType currencyCode, CancellationToken cancellationToken = default);

	Task UpdateCurrencyRoundCountAsync(int currencyRoundCount, CancellationToken cancellationToken = default);
}
