/*using Microsoft.AspNetCore.Mvc;
using static Fuse8_ByteMinds.SummerSchool.InternalApiA.Controllers.HealthCheckResult;

namespace Fuse8_ByteMinds.SummerSchool.InternalApiA.Controllers;

/// <summary>
/// Методы для проверки работоспособности PublicApiA
/// </summary>
[Route("healthcheck")]
public class HealthCheckController : ControllerBase
{
	/// <summary>
	/// Check that the API is working
	/// </summary>
	/// <param name="checkExternalApi">
	/// It is necessary to check the functionality of the external API. If FALSE or NULL, only the current API is checked
	/// </param>
	/// <response code="200">Returns if it was possible to access the API</response>
	/// <response code="400">Returns if failed to access the API</response>
	[HttpGet]
	public HealthCheckResult Check(bool? checkExternalApi) => new() { Status = CheckStatus.Ok, CheckedOn = DateTimeOffset.Now };
}

/// <summary>
/// API health check result
/// </summary>
public record HealthCheckResult
{
	/// <summary>
	/// Check date
	/// </summary>
	public DateTimeOffset CheckedOn { get; init; }

	/// <summary>
	/// API health status
	/// </summary>
	public CheckStatus Status { get; init; }

	/// <summary>
	/// API status
	/// </summary>
	public enum CheckStatus
	{
		/// <summary>
		/// API works
		/// </summary>
		Ok = 1,

		/// <summary>
		/// API error
		/// </summary>
		Failed = 2,
	}
}*/

