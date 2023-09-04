using Domain.Aggregates;

namespace Application.Internal.Interfaces.Background;

public interface IBackgroundTaskQueue
{
	ValueTask QueueAsync(CacheTask cacheTask);

	ValueTask<CacheTask> DequeueAsync(CancellationToken cancellationToken);
}