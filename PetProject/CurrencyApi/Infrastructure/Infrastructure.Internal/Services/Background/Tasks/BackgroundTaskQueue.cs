using Application.Internal.Interfaces.Background;
using Domain.Aggregates;
using System.Threading.Channels;

namespace Infrastructure.Internal.Services.Background.Tasks;

public sealed class BackgroundTaskQueue : IBackgroundTaskQueue
{
	private readonly Channel<CacheTask> _queue;

	public BackgroundTaskQueue()
	{
		BoundedChannelOptions options = new BoundedChannelOptions(32) { FullMode = BoundedChannelFullMode.Wait };
		_queue = Channel.CreateBounded<CacheTask>(options);
	}

	public ValueTask QueueAsync(CacheTask cacheTask)
	{
		return _queue.Writer.WriteAsync(cacheTask);
	}

	public ValueTask<CacheTask> DequeueAsync(CancellationToken cancellationToken)
	{
		return _queue.Reader.ReadAsync(cancellationToken);
	}
}