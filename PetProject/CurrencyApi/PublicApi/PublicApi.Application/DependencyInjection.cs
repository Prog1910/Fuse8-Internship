using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PublicApi.Domain.Options;

namespace PublicApi.Application;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<PublicApiOptions>(configuration.GetSection(PublicApiOptions.SectionName));
	}
}