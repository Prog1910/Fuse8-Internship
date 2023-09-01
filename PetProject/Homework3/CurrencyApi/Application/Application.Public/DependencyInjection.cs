﻿using Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Public;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<PublicApiOptions>(configuration.GetSection(PublicApiOptions.SectionName));

		return services;
	}
}