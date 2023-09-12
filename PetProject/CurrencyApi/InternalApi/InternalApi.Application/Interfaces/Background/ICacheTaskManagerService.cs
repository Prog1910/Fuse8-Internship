using Shared.Domain.Enums;

namespace InternalApi.Application.Interfaces.Background;

public interface ICacheTaskManagerService
{
	Task<Guid> RecalculateCacheAsync(CurrencyType baseCurrency, CancellationToken cancellationToken = default);
}