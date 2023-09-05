﻿using Application.Public;
using Infrastructure.Public;
using PublicApi;
using PublicApi.Middleware;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;

var builder = WebApplication.CreateBuilder(args);
{
	ConfigureSerilog(builder);

	builder.Services.AddPresentation(builder.Configuration)
		.AddInfrastructure(builder.Configuration)
		.AddApplication(builder.Configuration);

	builder.Services.AddHealthChecks();
}

var app = builder.Build();
{
	SetupSwagger(app);

	app.UseMiddleware<RequestLoggingMiddleware>();

	app.UseRouting().UseEndpoints(endpoints => endpoints.MapControllers());

	app.MapHealthChecks("/health");

	await app.RunAsync();
}
return;

static void SetupSwagger(WebApplication app)
{
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI(options =>
		{
			options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "v1");
			options.RoutePrefix = "swagger";
		});
	}
}

static void ConfigureSerilog(WebApplicationBuilder builder)
{
	builder.Host.UseSerilog(
		(context, _, configuration) => configuration.ReadFrom.Configuration(context.Configuration).Enrich
			.WithMachineName().Enrich
			.FromLogContext().Enrich
			.WithExceptionDetails(new DestructuringOptionsBuilder().WithDefaultDestructurers()));
}