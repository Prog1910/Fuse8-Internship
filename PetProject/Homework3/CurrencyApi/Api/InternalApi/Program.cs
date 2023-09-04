using Application.Internal;
using Infrastructure.Internal;
using Infrastructure.Internal.Services.Grpc;
using InternalApi;
using InternalApi.Middleware;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;

var builder = WebApplication.CreateBuilder();
{
	ConfigureSerilog(builder);

	builder.Services.AddPresentation()
		.AddInfrastructure(builder.Configuration)
		.AddApplication(builder.Configuration);

	builder.WebHost.UseKestrel((builderContext, options) =>
	{
		var grpcPort = builderContext.Configuration.GetValue<int>("GrpcPort");
		options.ConfigureEndpointDefaults(p =>
		{
			p.Protocols = p.IPEndPoint!.Port == grpcPort ? HttpProtocols.Http2 : HttpProtocols.Http1;
		});
	});
}

var app = builder.Build();
{
	SetupSwagger(app);

	app.UseMiddleware<RequestLoggingMiddleware>();

	app.UseRouting().UseEndpoints(endpoints => endpoints.MapControllers());

	SetupGrpcService(builder, app);

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