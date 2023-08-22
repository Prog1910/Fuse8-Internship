using Audit.Core;
using Audit.Http;
using CurrencyApi.InternalApi.Common.Mapping;
using CurrencyApi.InternalApi.Filters;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace CurrencyApi.InternalApi;

public static class DependencyInjection
{
	public static IServiceCollection AddPresentation(this IServiceCollection services)
	{
		ConfigureSerilog();

		services.AddGrpc();

		services.AddMappings();

		services.AddGlobalErrorsHandler();

		services.AddEndpointsApiExplorer();

		services.AddSwagger();

		return services;
	}

	private static void ConfigureSerilog()
	{
		Configuration.Setup()
			.UseSerilog(config => config.Message(auditEvent =>
			{
				if (auditEvent is AuditEventHttpClient httpClientEvent)
				{
					var contentBody = httpClientEvent.Action?.Response?.Content?.Body;
					if (contentBody is string { Length: > 1000 } stringBody)
					{
						httpClientEvent.Action!.Response.Content.Body = stringBody[..1000] + "<...>";
					}
				}
				return auditEvent.ToJson();
			}));
	}

	private static IServiceCollection AddGlobalErrorsHandler(this IServiceCollection services)
	{
		services.AddControllers(options => options.Filters.Add<GlobalErrorsHandler>())
					.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

		return services;
	}

	private static IServiceCollection AddSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo
			{
				Version = "v1",
				Title = "Internal API",
			});

			options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Program).Assembly.GetName().Name}.xml"));
		});

		return services;
	}
}