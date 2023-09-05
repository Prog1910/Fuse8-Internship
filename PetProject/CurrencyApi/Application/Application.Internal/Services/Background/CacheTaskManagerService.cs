using Application.Internal.Interfaces.Background;
using Application.Internal.Persistence;
using Domain.Aggregates;
using Domain.Enums;

namespace Application.Internal.Services.Background;

public sealed class CacheTaskManagerService : ICacheTaskManagerService
{
	private readonly ITaskRepository _repository;
	private readonly IBackgroundTaskQueue _taskQueue;

	public CacheTaskManagerService(ITaskRepository repository, IBackgroundTaskQueue taskQueue)
	{
		_repository = repository;
		_taskQueue = taskQueue;
	}

	public async Task<Guid> RecalculateCacheAsync(CurrencyType baseCurrency)
	{
		var cacheTask = CacheTask.Create(baseCurrency.ToString());
		_repository.AddCacheTask(cacheTask);
		await _taskQueue.QueueAsync(cacheTask);

		return cacheTask.Id;
	}
}