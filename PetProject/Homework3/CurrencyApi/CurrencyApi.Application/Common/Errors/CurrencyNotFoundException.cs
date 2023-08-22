using System.Net;

namespace CurrencyApi.Application.Common.Errors;

public sealed class CurrencyNotFoundException : HttpRequestException
{
	public CurrencyNotFoundException() : base(
		message: "Unknown currency",
		inner: default,
		HttpStatusCode.NotFound)
	{
	}
}