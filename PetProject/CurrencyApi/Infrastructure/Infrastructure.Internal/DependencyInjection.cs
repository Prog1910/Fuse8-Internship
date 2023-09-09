using Application.Internal.Interfaces.Persistence;
using Application.Internal.Interfaces.Rest;
using Infrastructure.Internal.Persistence;
using Infrastructure.Internal.Services.Rest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Internal;

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

		services.AddScoped<ICurDbContext>(provider => provider.GetRequiredService<CurDbContext>());
	}
}
