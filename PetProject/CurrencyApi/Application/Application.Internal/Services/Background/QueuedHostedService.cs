using Application.Internal.Interfaces.Background;
using Application.Internal.Interfaces.Rest;
using Application.Internal.Persistence;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Internal.Services.Background;

public sealed class QueuedHostedService : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<QueuedHostedService> _logger;
	private readonly IBackgroundTaskQueue _taskQueue;

	public QueuedHostedService(IBackgroundTaskQueue taskQueue, IServiceProvider serviceProvider, ILogger<QueuedHostedService> logger)
	{
		_taskQueue = taskQueue;
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using var scope = _serviceProvider.CreateScope();
		var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
		var incompleteTasks = taskRepo.GetAllTasks().Where(t => t.Status is not (CacheTaskStatus.CompletedSuccessfully or CacheTaskStatus.CompletedWithError or CacheTaskStatus.Cancelled)).ToList();
		if (incompleteTasks.FirstOrDefault() is { } taskToQueue)
		{
			await _taskQueue.QueueAsync(taskToQueue);
			foreach (var taskToCancel in incompleteTasks)
			{
				if (taskToCancel.Id.Equals(taskToQueue.Id)) continue;

				taskToCancel.Status = CacheTaskStatus.Cancelled;
			}
			await taskRepo.SaveChangesAsync();
		}

		while (stoppingToken.IsCancellationRequested is false)
		{
			try
			{
				var taskToComplete = await _taskQueue.DequeueAsync(stoppingToken);
				var recalculationService = scope.ServiceProvider.GetRequiredService<ICacheRecalculationService>();
				await recalculationService.RecalculateCacheAsync(taskToComplete.Id, stoppingToken);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, message: "An error occurred while recalculating cache.");
			}
		}
	}
}