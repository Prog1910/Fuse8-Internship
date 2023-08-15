using Application;
using Audit.Core;
using Audit.Http;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Filters;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Middleware;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi;

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

		services.AddControllers(options => options.Filters.Add<GlobalErrorsHandler>())
			.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo
			{
				Version = "v1",
				Title = "Public API",
			});

			options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Program).Assembly.GetName().Name}.xml"));
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
				options.RoutePrefix = "swagger";
			});
		}

		app.UseMiddleware<RequestsLoggerMiddleware>();

		app.UseRouting().UseEndpoints(endpoints => endpoints.MapControllers());
	}
}