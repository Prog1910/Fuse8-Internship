using Domain.Aggregates;

namespace Application.Internal.Persistence;

public interface ITaskRepository
{
	void AddCacheTask(CacheTask cacheTask);

	CacheTask GetCacheTaskById(Guid taskId);

	IEnumerable<CacheTask> GetAllTasks();

	void UpdateCacheTask(CacheTask cacheTask);
}