using Microsoft.AspNetCore.Mvc;
using static InternalApi.Controllers.HealthCheckResult;

namespace InternalApi.Controllers;

/// <summary>
///     Методы для проверки работоспособности PublicApiA
/// </summary>
[Route("healthcheck")]
public class HealthCheckController : ControllerBase
{
	/// <summary>
	///     Check that the API is working
	/// </summary>
	/// <param name="checkExternalApi">
	///     It is necessary to check the functionality of the external API. If FALSE or NULL, only the current API is checked
	/// </param>
	/// <response code="200">Returns if it was possible to access the API</response>
	/// <response code="400">Returns if failed to access the API</response>
	[HttpGet]
	public HealthCheckResult Check(bool? checkExternalApi)
	{
		if (checkExternalApi is false)
		{
		}
		else
		{
		}
		return new HealthCheckResult { Status = CheckStatus.Ok, CheckedOn = DateTimeOffset.Now };
	}
}

/// <summary>
///     API health check result
/// </summary>
public record HealthCheckResult
{
	/// <summary>
	///     API status
	/// </summary>
	public enum CheckStatus
	{
		/// <summary>
		///     API works
		/// </summary>
		Ok = 1,

		/// <summary>
		///     API error
		/// </summary>
		Failed = 2
	}

	/// <summary>
	///     Check date
	/// </summary>
	public DateTimeOffset CheckedOn { get; init; }

	/// <summary>
	///     API health status
	/// </summary>
	public CheckStatus Status { get; init; }
}