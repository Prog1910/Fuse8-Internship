namespace InternalApi.Application.Interfaces.Rest;

public interface ICacheRecalculationService
{
	Task RecalculateCacheAsync(Guid cacheTaskId, CancellationToken cancellationToken = default);
}
