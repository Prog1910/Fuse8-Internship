using Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Settings;
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
		services.Configure<CurrencyApiSettings>(_configuration.GetSection("CurrencyApi"));
		services.AddHttpClient<CurrencyController>();

		services.AddControllers()
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

		app.UseMiddleware<RequestLoggerMiddleware>();

		app.UseRouting()
			.UseEndpoints(endpoints => endpoints.MapControllers());
	}
}