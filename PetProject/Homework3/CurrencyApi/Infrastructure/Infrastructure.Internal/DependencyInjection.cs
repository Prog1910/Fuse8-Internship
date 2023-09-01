using Application.Internal.Persistence;
using Infrastructure.Internal.Persistence;
using Infrastructure.Internal.Persistence.Repositories;
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
		services.AddPersistence();

		services.AddInternalDbContext(configuration);

		return services;
	}

	private static IServiceCollection AddInternalDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<CurDbContext>(options =>
		{
			options.UseNpgsql(
					configuration.GetConnectionString("SummerSchool"),
					sqlOptionsBuilder =>
					{
						sqlOptionsBuilder.EnableRetryOnFailure();
						sqlOptionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, schema: "cur");
					})
				.EnableDetailedErrors()
				.ConfigureWarnings(p => p.Log(
										   (CoreEventId.StartedTracking, LogLevel.Information),
										   (RelationalEventId.TransactionRolledBack, LogLevel.Warning),
										   (RelationalEventId.CommandCanceled, LogLevel.Warning))
									   .Ignore(RelationalEventId.CommandCreated, RelationalEventId.ConnectionCreated));

			options.UseSnakeCaseNamingConvention();
		});

		return services;
	}

	private static IServiceCollection AddPersistence(this IServiceCollection services)
	{
		services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();

		return services;
	}
}