using System.Net;

namespace Shared.Domain.Errors;

public sealed class ApiRequestLimitException : HttpRequestException
{
	public ApiRequestLimitException() : base("You have reached your request limit", default, HttpStatusCode.TooManyRequests)
	{
	}
}
