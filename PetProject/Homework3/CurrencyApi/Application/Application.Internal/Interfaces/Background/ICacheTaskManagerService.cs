using Domain.Enums;

namespace Application.Internal.Interfaces.Background;

public interface ICacheTaskManagerService
{
	Task<Guid> RecalculateCurrencyCacheAsync(CurrencyType baseCurrency);
}