using System.Threading.Channels;
using InternalApi.Application.Interfaces.Background;
using InternalApi.Domain.Aggregates;

namespace InternalApi.Application.Services.Background.Tasks;

public sealed class BackgroundTaskQueue : IBackgroundTaskQueue
{
	private readonly Channel<CacheTask> _queue;

	public BackgroundTaskQueue()
	{
		BoundedChannelOptions options = new(32) { FullMode = BoundedChannelFullMode.Wait };
		_queue = Channel.CreateBounded<CacheTask>(options);
	}

	public ValueTask QueueAsync(CacheTask cacheTask, CancellationToken cancellationToken)
	{
		return _queue.Writer.WriteAsync(cacheTask, cancellationToken);
	}

	public ValueTask<CacheTask> DequeueAsync(CancellationToken cancellationToken)
	{
		return _queue.Reader.ReadAsync(cancellationToken);
	}
}