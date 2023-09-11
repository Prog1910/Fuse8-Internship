using Audit.Http;
using Domain.Options;
using InternalApi.Application.Interfaces.Background;
using InternalApi.Application.Interfaces.Rest;
using InternalApi.Application.Services.Background;
using InternalApi.Application.Services.Background.Tasks;
using InternalApi.Application.Services.Rest;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace InternalApi.Application;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<InternalApiOptions>(configuration.GetSection(InternalApiOptions.SectionName));

		services.AddBackgroundServices();

		services.AddRestServices();
	}

	private static void AddBackgroundServices(this IServiceCollection services)
	{
		services.AddScoped<ICacheTaskManagerService, CacheTaskManagerService>();
		services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
		services.AddHostedService<QueuedHostedService>();
	}

	private static void AddRestServices(this IServiceCollection services)
	{
		services.AddScoped<ICacheCurrencyApi, CacheCurrencyService>();
		services.AddCurrencyHttpClient();
	}

	private static void AddCurrencyHttpClient(this IServiceCollection services)
	{
		services.AddHttpClient<ICurrencyApi, CurrencyService>("CurrencyClient")
			.AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
				.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) - 1)))
			.AddAuditHandler(audit => audit.IncludeRequestHeaders()
				.IncludeRequestBody()
				.IncludeResponseHeaders()
				.IncludeResponseBody()
				.IncludeContentHeaders());
	}
}
