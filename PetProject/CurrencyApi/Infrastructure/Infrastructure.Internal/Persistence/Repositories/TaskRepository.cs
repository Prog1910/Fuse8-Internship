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
	}

	public async Task AddCacheTask(CacheTask cacheTask)
	{
		await _tasks.AddAsync(cacheTask);
		await _context.SaveChangesAsync();
	}

	public Task<CacheTask?> GetCacheTaskById(Guid taskId)
		=> _tasks.SingleOrDefaultAsync(t => t.Id.Equals(taskId)) ?? throw new Exception("Cache task not found.");

	public IEnumerable<CacheTask> GetAllTasks()
		=> _tasks.AsEnumerable();

	public async Task SaveChangesAsync()
		=> await _context.SaveChangesAsync();
}