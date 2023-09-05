using System.Net;

namespace Domain.Errors;

public sealed class CurrencyNotFoundException : HttpRequestException
{
	public CurrencyNotFoundException() : base(message: "Unknown currency", inner: default, HttpStatusCode.NotFound)
	{
	}
}