using Audit.Core;
using Audit.Http;
using InternalApi.Filters;
using InternalApi.Mapping;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace InternalApi;

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

		services.AddHealthChecks();

		return services;
	}

	private static void ConfigureSerilog()
	{
		Configuration.Setup()
			.UseSerilog(config => config.Message(auditEvent =>
			{
				if (auditEvent is not AuditEventHttpClient httpClientEvent) return auditEvent.ToJson();

				var contentBody = httpClientEvent.Action?.Response?.Content?.Body;
				if (contentBody is not string { Length: > 1000 } stringBody) return auditEvent.ToJson();

				var responseContent = httpClientEvent.Action!.Response?.Content;
				if (responseContent is not null) responseContent.Body = stringBody[..1000] + "<...>";
				return auditEvent.ToJson();
			}));
	}

	private static void AddGlobalErrorsHandler(this IServiceCollection services)
	{
		services.AddControllers(options => options.Filters.Add<GlobalErrorsHandler>())
			.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
	}

	private static void AddSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc(name: "v1", new OpenApiInfo
			{
				Version = "v1",
				Title = "Internal API"
			});

			options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Program).Assembly.GetName().Name}.xml"));
		});
	}
}