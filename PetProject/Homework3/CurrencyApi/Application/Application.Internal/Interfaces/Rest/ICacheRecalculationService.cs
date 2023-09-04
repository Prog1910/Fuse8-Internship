namespace Application.Internal.Interfaces.Rest;

public interface ICacheRecalculationService
{
	Task RecalculateCacheAsync(Guid cacheTaskId, CancellationToken cancellationToken);
}