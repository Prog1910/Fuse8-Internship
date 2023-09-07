using Domain.Enums;

namespace Application.Public.Interfaces.Rest;

public interface ISettingsService
{
	CurrencyType DefaultCurrencyCode { get; set; }

	int CurrencyRoundCount { get; set; }
}
