using System.Reflection;
using Mapster;
using MapsterMapper;

namespace Api.Internal.Mapping;

public static class DependencyInjection
{
	public static IServiceCollection AddMappings(this IServiceCollection services)
	{
		TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;
		config.Scan(Assembly.GetExecutingAssembly());

		services.AddSingleton(config);
		services.AddScoped<IMapper, ServiceMapper>();

		return services;
	}
}
