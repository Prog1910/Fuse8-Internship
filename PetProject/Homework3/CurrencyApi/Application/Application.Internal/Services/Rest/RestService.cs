using Application.Internal.Interfaces.Background;
using Application.Internal.Interfaces.Rest;
using Application.Internal.Persistence;
using Domain.Aggregates;
using Domain.Enums;

namespace Application.Internal.Services.Rest;

public sealed class RestService : IRestApi
{
	private readonly ITaskRepository _repository;
	private readonly IBackgroundTaskQueue _taskQueue;

	public RestService(ITaskRepository repository, IBackgroundTaskQueue taskQueue)
	{
		_repository = repository;
		_taskQueue = taskQueue;
	}

	public async Task<Guid> RecalculateCurrencyCacheAsync(CurrencyType baseCurrency)
	{
		var cacheTask = CacheTask.Create(baseCurrency.ToString());
		_repository.AddCacheTask(cacheTask);
		await _taskQueue.QueueAsync(cacheTask);
		return cacheTask.Id;
	}
}