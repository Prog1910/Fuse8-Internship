using Application.Internal.Interfaces.Background;
using Application.Internal.Interfaces.Persistence;
using Domain.Aggregates;
using Domain.Enums;

namespace Application.Internal.Services.Background;

public sealed class CacheTaskManagerService : ICacheTaskManagerService
{
	private readonly ICurDbContext _curDbContext;
	private readonly IBackgroundTaskQueue _taskQueue;

	public CacheTaskManagerService(ICurDbContext curDbContext, IBackgroundTaskQueue taskQueue)
	{
		_curDbContext = curDbContext;
		_taskQueue = taskQueue;
	}

	public async Task<Guid> RecalculateCacheAsync(CurrencyType baseCurrency)
	{
		CacheTask cacheTask = CacheTask.Create(baseCurrency.ToString());

		_curDbContext.CacheTasks.Add(cacheTask);
		await _curDbContext.SaveChangesAsync();

		await _taskQueue.QueueAsync(cacheTask);

		return cacheTask.Id;
	}
}
