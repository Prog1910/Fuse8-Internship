using Application.Internal.Interfaces.Rest;
using Application.Internal.Services.Rest;
using Audit.Http;
using Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Application.Internal;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<InternalApiOptions>(configuration.GetSection(InternalApiOptions.SectionName));

		services.AddRestServices();
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
								  .WaitAndRetryAsync(retryCount: 3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(x: 2, retryAttempt) - 1)))
			.AddAuditHandler(audit => audit.IncludeRequestHeaders()
								 .IncludeRequestBody()
								 .IncludeResponseHeaders()
								 .IncludeResponseBody()
								 .IncludeContentHeaders());
	}
}