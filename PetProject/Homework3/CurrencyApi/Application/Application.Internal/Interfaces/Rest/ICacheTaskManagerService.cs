using Domain.Enums;

namespace Application.Internal.Interfaces.Rest;

public interface ICacheTaskManagerService
{
	Task<Guid> RecalculateCurrencyCacheAsync(CurrencyType baseCurrency);
}