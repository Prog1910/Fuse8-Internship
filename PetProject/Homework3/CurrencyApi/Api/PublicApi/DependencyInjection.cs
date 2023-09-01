﻿using Audit.Core;
using Audit.Http;
using Microsoft.OpenApi.Models;
using PublicApi.Filters;
using PublicApi.Mapping;
using System.Text.Json.Serialization;

namespace PublicApi;

public static class DependencyInjection
{
	public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
	{
		ConfigureSerilog();

		services.AddGrpcClient(configuration);

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

	// TODO: не нравится, что он тут. Нужно будет переместить
	private static IServiceCollection AddGrpcClient(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddGrpcClient<Protos.CurrencyGrpc.CurrencyGrpcClient>(o => o.Address = new Uri(configuration["InternalApiA:BaseUrl"] ?? "http://localhost:5000"))
			.AddAuditHandler(audit => audit.IncludeRequestBody());

		return services;
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
			options.SwaggerDoc(name: "v1", new OpenApiInfo
			{
				Version = "v1",
				Title = "Public API"
			});

			options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Program).Assembly.GetName().Name}.xml"));
		});

		return services;
	}
}