using Fuse8_ByteMinds.SummerSchool.Domain.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Fuse8_ByteMinds.SummerSchool.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.Configure<CurrencyServiceOptions>(configuration.GetSection(CurrencyServiceOptions.SectionName));
		return services;
	}
}
