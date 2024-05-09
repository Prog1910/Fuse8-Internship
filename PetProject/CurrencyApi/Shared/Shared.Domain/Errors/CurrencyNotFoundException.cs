using System.Net;

namespace Shared.Domain.Errors;

public sealed class CurrencyNotFoundException : HttpRequestException
{
	public CurrencyNotFoundException() : base("Unknown currency", default, HttpStatusCode.NotFound)
	{
	}
}
