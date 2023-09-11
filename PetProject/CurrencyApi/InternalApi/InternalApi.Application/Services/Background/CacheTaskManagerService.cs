using Domain.Aggregates;
using Domain.Enums;
using InternalApi.Application.Interfaces.Background;
using InternalApi.Application.Interfaces.Persistence;

namespace InternalApi.Application.Services.Background;

public sealed class CacheTaskManagerService : ICacheTaskManagerService
{
	private readonly ICurDbContext _curDbContext;
	private readonly IBackgroundTaskQueue _taskQueue;

	public CacheTaskManagerService(ICurDbContext curDbContext, IBackgroundTaskQueue taskQueue)
	{
		_curDbContext = curDbContext;
		_taskQueue = taskQueue;
	}

	public async Task<Guid> RecalculateCacheAsync(CurrencyType baseCurrency, CancellationToken cancellationToken)
	{
		CacheTask cacheTask = CacheTask.Create(baseCurrency.ToString());

		await _curDbContext.CacheTasks.AddAsync(cacheTask, cancellationToken);
		await _curDbContext.SaveChangesAsync(cancellationToken);

		await _taskQueue.QueueAsync(cacheTask, cancellationToken);

		return cacheTask.Id;
	}
}
