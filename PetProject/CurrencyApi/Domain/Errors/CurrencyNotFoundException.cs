using System.Net;

namespace Domain.Errors;

public sealed class CurrencyNotFoundException : HttpRequestException
{
	public CurrencyNotFoundException() : base("Unknown currency", default, HttpStatusCode.NotFound)
	{
	}
}
