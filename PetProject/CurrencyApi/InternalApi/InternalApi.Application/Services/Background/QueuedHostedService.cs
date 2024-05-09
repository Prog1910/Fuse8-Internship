using InternalApi.Application.Interfaces.Background;
using InternalApi.Application.Interfaces.Rest;
using InternalApi.Domain.Aggregates;
using InternalApi.Domain.Enums;
using InternalApi.Domain.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InternalApi.Application.Services.Background;

public sealed class QueuedHostedService : BackgroundService
{
	private readonly ILogger<QueuedHostedService> _logger;
	private readonly IServiceProvider _services;
	private readonly IBackgroundTaskQueue _taskQueue;

	public QueuedHostedService(IBackgroundTaskQueue taskQueue, IServiceProvider services, ILogger<QueuedHostedService> logger)
	{
		_taskQueue = taskQueue;
		_services = services;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using IServiceScope scope = _services.CreateScope();
		CurDbContext curDbContext = scope.ServiceProvider.GetRequiredService<CurDbContext>();
		List<CacheTask> incompleteTasks =
			curDbContext.CacheTasks.Where(t => t.Status == CacheTaskStatus.Created || t.Status == CacheTaskStatus.InProgress).ToList();
		if (incompleteTasks.FirstOrDefault() is { } taskToQueue)
		{
			await _taskQueue.QueueAsync(taskToQueue, stoppingToken);
			foreach (CacheTask taskToCancel in incompleteTasks.Where(taskToCancel => taskToCancel.Id.Equals(taskToQueue.Id) == false))
			{
				taskToCancel.Status = CacheTaskStatus.Cancelled;
			}

			await curDbContext.SaveChangesAsync(stoppingToken);
		}

		while (stoppingToken.IsCancellationRequested is false)
		{
			try
			{
				CacheTask taskToComplete = await _taskQueue.DequeueAsync(stoppingToken);
				ICacheRecalculationService recalculationService = scope.ServiceProvider.GetRequiredService<ICacheRecalculationService>();
				await recalculationService.RecalculateCacheAsync(taskToComplete.Id, stoppingToken);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while recalculating cache.");
			}
		}
	}
}
