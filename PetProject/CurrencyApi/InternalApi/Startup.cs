using Audit.Core;
using Audit.Http;
using Fuse8_ByteMinds.SummerSchool.Application;
using Fuse8_ByteMinds.SummerSchool.Application.Filters;
using Fuse8_ByteMinds.SummerSchool.Application.Middleware;
using Fuse8_ByteMinds.SummerSchool.Application.Services;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.InternalApi;

public class Startup
{
	private readonly IConfiguration _configuration;

	public Startup(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddApplication(_configuration);

		services.AddHttpClient<ICurrencyService, CurrencyService>("Client1")
				.AddPolicyHandler(HttpPolicyExtensions
					.HandleTransientHttpError()
					.WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) - 1))
				)
				.AddAuditHandler(audit => audit
					.IncludeRequestHeaders()
					.IncludeRequestBody()
					.IncludeResponseHeaders()
					.IncludeResponseBody()
					.IncludeContentHeaders()
					);

		Configuration.Setup()
			.UseSerilog(config => config.Message(
				auditEvent =>
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
				})
			);

		services.AddControllers(
			options => options.Filters.Add<GlobalExceptionsHandler>()
			)
			// Добавляем глобальные настройки для преобразования Json
			.AddJsonOptions(
				options =>
				{
					// Добавляем конвертер для енама
					// По умолчанию енам преобразуется в цифровое значение
					// Этим конвертером задаем перевод в строковое занчение
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				});

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo
			{
				Version = "v1",
				Title = "Public API",
			});

			var xmlFilename = $"{typeof(Program).Assembly.GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
			options.IncludeXmlComments(xmlPath);
		});
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
				options.RoutePrefix = string.Empty;
			});
		}

		app.UseMiddleware<RequestLoggingMiddleware>();

		app.UseRouting()
			.UseEndpoints(endpoints => endpoints.MapControllers());
	}
}