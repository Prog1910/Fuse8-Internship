using Domain.Enums;

namespace Application.Internal.Interfaces.Background;

public interface ICacheTaskManagerService
{
	Task<Guid> RecalculateCacheAsync(CurrencyType baseCurrency, CancellationToken cancellationToken = default);
}
