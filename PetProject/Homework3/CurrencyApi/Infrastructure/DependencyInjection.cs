using Application.Common.Interfaces.Rest;
using Application.Persistence;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services.Rest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructureToInternalApi(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddPersistenceToInternalApi();

		services.AddInternalDbContext(configuration);

		return services;
	}

	private static IServiceCollection AddInternalDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<InternalDbContext>(options =>
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

	public static IServiceCollection AddInfrastructureToPublicApi(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<IInternalApi, InternalService>();
		services.AddScoped<IFavoriteCurrencyService, FavoriteCurrencyService>();
		services.AddScoped<ISettingsWriterService, SettingsWriterService>();
		services.AddScoped<ISettingsReaderService, SettingsReaderService>();

		services.AddPersistenceToPublicApi();

		services.AddPublicDbContext(configuration);

		return services;
	}

	private static IServiceCollection AddPublicDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<PublicDbContext>(options =>
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

	private static IServiceCollection AddPersistenceToInternalApi(this IServiceCollection services)
	{
		services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();

		return services;
	}

	private static IServiceCollection AddPersistenceToPublicApi(this IServiceCollection services)
	{
		services.AddScoped<ISettingsRepository, SettingsRepository>();
		services.AddScoped<IFavoriteCurrenciesRepository, FavoriteCurrenciesRepository>();

		return services;
	}
}