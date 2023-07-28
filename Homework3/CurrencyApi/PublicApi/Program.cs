using Fuse8_ByteMinds.SummerSchool.PublicApi;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;

var webHost = Host
	.CreateDefaultBuilder()
	.ConfigureAppConfiguration(config =>
		config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
		)
	.UseSerilog((context, services, configuration) =>
		configuration
		.ReadFrom.Configuration(context.Configuration)
		.Enrich.WithMachineName()
		.Enrich.FromLogContext()
		.Enrich.WithExceptionDetails(new DestructuringOptionsBuilder().WithDefaultDestructurers())
		)
	.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
	.Build();

await webHost.RunAsync();
