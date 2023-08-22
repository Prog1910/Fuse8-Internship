using CurrencyApi.Application.Common.Services.Interfaces;
using CurrencyApi.Application.Persistence;
using CurrencyApi.Infrastructure.Persistence;
using CurrencyApi.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyApi.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructureToInternalApi(this IServiceCollection services)
	{
		services.AddPersistence();

		return services;
	}

	public static IServiceCollection AddInfrastructureToPublicApi(this IServiceCollection services)
	{
		services.AddServices();

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