using Application.Services;
using Audit.Http;
using Domain.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<CurrencyServiceOptions>(configuration.GetSection(CurrencyServiceOptions.SectionName));

		services.AddHttpClient<ICurrencyService, CurrencyService>("Client1")
					.AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
	 .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) - 1)))
					.AddAuditHandler(audit => audit.IncludeRequestHeaders()
	 .IncludeRequestBody()
	 .IncludeResponseHeaders()
	 .IncludeResponseBody()
	 .IncludeContentHeaders());

		return services;
	}
}