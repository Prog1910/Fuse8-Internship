using System.Reflection;
using InternalApi.Application.Interfaces.Rest;
using InternalApi.Domain.Persistence;
using InternalApi.Infrastructure.Services.Rest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InternalApi.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddPersistence(configuration);

		services.AddRestServices();

		return services;
	}

	private static void AddRestServices(this IServiceCollection services)
	{
		services.AddScoped<ICacheRecalculationService, CacheRecalculationService>();
	}

	private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<CurDbContext>(options =>
		{
			options.UseNpgsql(
					configuration.GetConnectionString("SummerSchool"),
					sqlOptionsBuilder =>
					{
						sqlOptionsBuilder.MigrationsAssembly(Assembly.GetExecutingAssembly().ToString());
						sqlOptionsBuilder.EnableRetryOnFailure();
						sqlOptionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "cur");
					})
				.UseAllCheckConstraints()
				.EnableDetailedErrors()
				.ConfigureWarnings(p => p.Log(
						(CoreEventId.StartedTracking, LogLevel.Information),
						(RelationalEventId.TransactionRolledBack, LogLevel.Warning),
						(RelationalEventId.CommandCanceled, LogLevel.Warning))
					.Ignore(RelationalEventId.CommandCreated, RelationalEventId.ConnectionCreated));
			options.UseSnakeCaseNamingConvention();
		});

		services.AddScoped<DbContext, CurDbContext>();
	}
}
