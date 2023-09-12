using InternalApi.Domain.Aggregates;

namespace InternalApi.Application.Interfaces.Background;

public interface IBackgroundTaskQueue
{
	ValueTask QueueAsync(CacheTask cacheTask, CancellationToken cancellationToken = default);

	ValueTask<CacheTask> DequeueAsync(CancellationToken cancellationToken = default);
}
