using InternalApi.Api;
using InternalApi.Api.Middleware;
using InternalApi.Application;
using InternalApi.Infrastructure;
using InternalApi.Infrastructure.Persistence;
using InternalApi.Infrastructure.Services.Grpc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;

WebApplicationBuilder builder = WebApplication.CreateBuilder();
{
	ConfigureSerilog(builder);

	builder.Services.AddPresentation()
		.AddInfrastructure(builder.Configuration)
		.AddApplication(builder.Configuration);

	builder.Services.AddHealthChecks()
		.AddNpgSql(builder.Configuration.GetConnectionString("SummerSchool")!);

	builder.WebHost.UseKestrel((builderContext, options) =>
	{
		int grpcPort = builderContext.Configuration.GetValue<int>("GrpcPort");
		options.ConfigureEndpointDefaults(p => { p.Protocols = p.IPEndPoint!.Port == grpcPort ? HttpProtocols.Http2 : HttpProtocols.Http1; });
	});
}

WebApplication app = builder.Build();
{
	SetupSwagger(app);

	app.UseMiddleware<RequestLoggingMiddleware>();

	app.UseRouting().UseEndpoints(endpoints => endpoints.MapControllers());

	app.MapHealthChecks("/health");

	SetupGrpcService(builder, app);

	using IServiceScope scope = app.Services.CreateScope();
	{
		IServiceProvider services = scope.ServiceProvider;
		CurDbContext context = services.GetRequiredService<CurDbContext>();
		if (context.Database.GetPendingMigrations().Any()) context.Database.Migrate();
	}

	await app.RunAsync();
}
return;

static void SetupGrpcService(WebApplicationBuilder builder, IApplicationBuilder app)
{
	app.UseWhen(context => context.Connection.LocalPort == builder.Configuration.GetValue<int>("GrpcPort"),
		grpcBuilder =>
		{
			grpcBuilder.UseRouting();
			grpcBuilder.UseEndpoints(endpoints => endpoints.MapGrpcService<CurrencyGrpcService>());
		});
}

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
