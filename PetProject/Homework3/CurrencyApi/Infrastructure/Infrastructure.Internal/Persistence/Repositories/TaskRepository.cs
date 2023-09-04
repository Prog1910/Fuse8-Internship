using Application.Internal.Persistence;
using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Internal.Persistence.Repositories;

public sealed class TaskRepository : ITaskRepository
{
	private readonly CurDbContext _context;
	private readonly DbSet<CacheTask> _tasks;

	public TaskRepository(CurDbContext context)
	{
		_context = context;
		_tasks = _context.CacheTasks ?? throw new Exception("Cache tasks not found.");
		_tasks.AsNoTracking();
	}

	public void AddCacheTask(CacheTask cacheTask)
	{
		_tasks.Add(cacheTask);
		_context.SaveChanges();
	}

	public CacheTask GetCacheTaskById(Guid taskId)
	{
		var cacheTask = _tasks.SingleOrDefault(t => t.Id.Equals(taskId)) ?? throw new Exception("Cache task not found.");
		return cacheTask;
	}

	public IEnumerable<CacheTask> GetAllTasks()
	{
		var tasks = _tasks.AsEnumerable();

		return tasks;
	}

	public void UpdateCacheTask(CacheTask cacheTask)
	{
		_tasks.Update(cacheTask);
		_context.SaveChanges();
	}
}