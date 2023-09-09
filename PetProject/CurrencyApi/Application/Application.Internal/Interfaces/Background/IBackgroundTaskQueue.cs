using Domain.Aggregates;

namespace Application.Internal.Interfaces.Background;

public interface IBackgroundTaskQueue
{
	ValueTask QueueAsync(CacheTask cacheTask, CancellationToken cancellationToken = default);

	ValueTask<CacheTask> DequeueAsync(CancellationToken cancellationToken = default);
}
