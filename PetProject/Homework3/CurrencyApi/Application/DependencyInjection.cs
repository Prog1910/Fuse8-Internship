﻿using Application.Common.Interfaces;
using Application.Common.Services;
using Audit.Http;
using Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationToInternalApi(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<InternalApiOptions>(configuration.GetSection(InternalApiOptions.SectionName));

		services.AddCachedCurrencyClient();

		services.AddCurrencyClient();

		return services;
	}

	public static IServiceCollection AddApplicationToPublicApi(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<PublicApiOptions>(configuration.GetSection(PublicApiOptions.SectionName));

		return services;
	}

	private static IServiceCollection AddCurrencyClient(this IServiceCollection services)
	{
		services.AddHttpClient<ICurrencyApi, CurrencyService>("CurrencyClient")
							.AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
				.WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) - 1)))
							.AddAuditHandler(audit => audit.IncludeRequestHeaders()
				.IncludeRequestBody()
				.IncludeResponseHeaders()
				.IncludeResponseBody()
				.IncludeContentHeaders());

		return services;
	}

	private static IServiceCollection AddCachedCurrencyClient(this IServiceCollection services)
	{
		services.AddScoped<ICachedCurrencyApi, CachedCurrencyService>();

		return services;
	}
}