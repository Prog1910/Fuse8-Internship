﻿using Microsoft.EntityFrameworkCore;
using PublicApi.Api;
using PublicApi.Api.Middleware;
using PublicApi.Application;
using PublicApi.Domain.Persistence;
using PublicApi.Infrastructure;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
	ConfigureSerilog(builder);

	builder.Services.AddPresentation(builder.Configuration)
		.AddInfrastructure(builder.Configuration)
		.AddApplication(builder.Configuration);

	builder.Services.AddHealthChecks()
		.AddNpgSql(builder.Configuration.GetConnectionString("SummerSchool")!);
}

WebApplication app = builder.Build();
{
	SetupSwagger(app);

	app.UseMiddleware<RequestLoggingMiddleware>();

	app.UseRouting().UseEndpoints(endpoints => endpoints.MapControllers());

	app.MapHealthChecks("/health");

	using IServiceScope scope = app.Services.CreateScope();
	{
		IServiceProvider services = scope.ServiceProvider;
		UserDbContext context = services.GetRequiredService<UserDbContext>();
		if (context.Database.GetPendingMigrations().Any()) context.Database.Migrate();
	}

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
			options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
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
