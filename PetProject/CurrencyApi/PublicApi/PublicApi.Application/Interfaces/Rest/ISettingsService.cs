using Domain.Enums;

namespace PublicApi.Application.Interfaces.Rest;

public interface ISettingsService
{
	CurrencyType DefaultCurrencyCode { get; set; }

	int CurrencyRoundCount { get; set; }
}
