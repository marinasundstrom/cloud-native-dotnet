using Serilog;
using Worker.Extensions;
using BlazorApp1.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) =>  cfg.ReadFrom.Configuration(builder.Configuration)
                        .Enrich.WithProperty("Application", ctx.HostingEnvironment.ApplicationName)
                        .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName));

builder.Services
    .AddMassTransit()
    .AddObservability("BlazorApp1.Worker", "1.0", builder.Configuration)
    .AddCaching(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapObservability();

app.MapGet("/", () => "Hello World!");

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.Run();
