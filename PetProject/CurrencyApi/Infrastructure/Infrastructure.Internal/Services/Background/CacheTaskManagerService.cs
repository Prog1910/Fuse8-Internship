using Application.Internal.Interfaces.Background;
using Domain.Aggregates;
using Domain.Enums;
using Infrastructure.Internal.Persistence;

namespace Infrastructure.Internal.Services.Background;

public sealed class CacheTaskManagerService : ICacheTaskManagerService
{
	private readonly CurDbContext _curDbContext;
	private readonly IBackgroundTaskQueue _taskQueue;

	public CacheTaskManagerService(CurDbContext curDbContext, IBackgroundTaskQueue taskQueue)
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