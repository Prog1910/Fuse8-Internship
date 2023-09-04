using Domain.Options;
using Mapster;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RestSharp;

namespace Infrastructure.Internal.Services.HealthCheck;

public sealed class CurrencyApiHealthCheck : IHealthCheck
{
	private readonly InternalApiOptions _internalApiOptions;

	public CurrencyApiHealthCheck(IOptions<InternalApiOptions> internalApiOptions)
	{
		_internalApiOptions = internalApiOptions.Value;
	}

	public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
	{
		var baseUrl = _internalApiOptions.BaseUrl;

		var client = new RestClient();
		var request = new RestRequest(baseUrl, Method.Get);
		throw new NotImplementedException();
	}
}