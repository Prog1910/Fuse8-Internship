using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Filters;

public class GlobalExceptionsHandler : IExceptionFilter
{
	private readonly ILogger<RequestLoggerMiddleware> _logger;

	public GlobalExceptionsHandler(ILogger<RequestLoggerMiddleware> logger) => _logger = logger;

	public void OnException(ExceptionContext context)
	{
		switch (context.Exception)
		{
			case ApiRequestLimitException:
				context.Result = new StatusCodeResult((int)HttpStatusCode.TooManyRequests);
				break;
			case CurrencyNotFoundException:
				context.Result = new StatusCodeResult((int)HttpStatusCode.NotFound);
				_logger.LogError(context.Exception, context.Exception.Message);
				break;
			default:
				context.Result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);
				_logger.LogError(context.Exception, context.Exception.Message);
				break;
		};
	}
}
