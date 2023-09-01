using Application.Public.Interfaces.Rest;
using Application.Public.Persistence;
using Infrastructure.Public.Persistence;
using Infrastructure.Public.Persistence.Repositories;
using Infrastructure.Public.Services.Rest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Public;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<IInternalApi, InternalService>();
		services.AddScoped<IFavoriteCurrencyService, FavoriteCurrencyService>();
		services.AddScoped<ISettingsWriterService, SettingsWriterService>();
		services.AddScoped<ISettingsReaderService, SettingsReaderService>();

		services.AddPersistence();

		services.AddPublicDbContext(configuration);

		return services;
	}

	private static IServiceCollection AddPublicDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<UserDbContext>(options =>
		{
			options.UseNpgsql(
					configuration.GetConnectionString("SummerSchool"),
					sqlOptionsBuilder =>
					{
						sqlOptionsBuilder.EnableRetryOnFailure();
						sqlOptionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, schema: "user");
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

		return services;
	}

	private static IServiceCollection AddPersistence(this IServiceCollection services)
	{
		services.AddScoped<ISettingsRepository, SettingsRepository>();
		services.AddScoped<IFavoriteCurrenciesRepository, FavoriteCurrenciesRepository>();

		return services;
	}
}