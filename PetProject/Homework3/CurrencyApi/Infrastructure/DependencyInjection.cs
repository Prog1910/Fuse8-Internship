using Application.Common.Interfaces;
using Application.Persistence;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructureToInternalApi(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddPersistence();

		services.AddDbContext<SummerSchoolDbContext>(options =>
		 {
			 options.UseNpgsql(
				connectionString: configuration.GetConnectionString("SummerSchool"),
				npgsqlOptionsAction: sqlOptionsBuilder =>
				{
					sqlOptionsBuilder.EnableRetryOnFailure();
					sqlOptionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "cur");
				});
			 options.UseSnakeCaseNamingConvention();
		 });

		return services;
	}

	public static IServiceCollection AddInfrastructureToPublicApi(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddServices();

		services.AddDbContext<SummerSchoolDbContext>(options =>
				 {
					 options.UseNpgsql(
						connectionString: configuration.GetConnectionString("SummerSchool"),
						npgsqlOptionsAction: sqlOptionsBuilder =>
						{
							sqlOptionsBuilder.EnableRetryOnFailure();
							sqlOptionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "user");
						});
					 options.UseSnakeCaseNamingConvention();
				 });

		return services;
	}

	private static IServiceCollection AddPersistence(this IServiceCollection services)
	{
		services.AddScoped<ICurrencyRepository, CurrencyRepository>();

		return services;
	}

	private static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddScoped<IInternalApi, InternalService>();

		return services;
	}
}
