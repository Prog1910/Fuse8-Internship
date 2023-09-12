using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PublicApi.Application.Interfaces.Rest;
using PublicApi.Domain.Persistence;
using PublicApi.Infrastructure.Services.Rest;

namespace PublicApi.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddRestServices();

		services.AddPersistence(configuration);

		return services;
	}

	private static void AddRestServices(this IServiceCollection services)
	{
		services.AddScoped<IInternalApi, InternalService>();
		services.AddScoped<IFavoritesService, FavoritesService>();
		services.AddScoped<ISettingsService, SettingsService>();
	}

	private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<UserDbContext>(options =>
		{
			options.UseNpgsql(
					configuration.GetConnectionString("SummerSchool"),
					sqlOptionsBuilder =>
					{
						sqlOptionsBuilder.MigrationsAssembly(Assembly.GetExecutingAssembly().ToString());
						sqlOptionsBuilder.EnableRetryOnFailure();
						sqlOptionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "user");
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

		services.AddScoped<DbContext, UserDbContext>();
	}
}
