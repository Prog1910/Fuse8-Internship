using System.Net;

namespace Domain.Errors;

public sealed class ApiRequestLimitException : HttpRequestException
{
	public ApiRequestLimitException() : base(message: "You have reached your request limit", inner: default, HttpStatusCode.TooManyRequests)
	{
	}
}
