using HealthChecks.UI.Client;
using Microsoft.EntityFrameworkCore;
using NSwag.AspNetCore;
using BlazorApp1.Extensions;
using BlazorApp1.Infrastructure.Persistence;
using BlazorApp1.Web.Extensions;
using BlazorApp1.Web.Middleware;
using BlazorApp1.Web.Services;
using Serilog;
using BlazorApp1.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) =>  cfg.ReadFrom.Configuration(builder.Configuration)
                        .Enrich.WithProperty("Application", ctx.HostingEnvironment.ApplicationName)
                        .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName));

var configuration = builder.Configuration;

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddCorsService()
    .AddHttpContextAccessor()
    .AddApiVersioningServices()
    .AddOpenApi(documentTitle: "BlazorApp1 API")
    .AddHealthChecksServices()
    .AddCaching(configuration)
    .AddAuthenticationServices()
    .AddFeatureManagement()
    .AddUniverse(configuration)
    .AddMassTransit()
    .AddObservability("BlazorApp1.Web", "1.0", configuration)
    .AddRateLimiter()
    .AddSignalR();

builder.Services
    .AddScoped<ICurrentUserServiceInternal, CurrentUserService>()
    .AddScoped<ICurrentUserService>(sp => sp.GetRequiredService<ICurrentUserServiceInternal>())
    .AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

app.MapHealthChecks("/healthz", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseSerilogRequestLogging();

app.MapObservability();

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors(CorsExtensions.MyAllowSpecificOrigins);

app.UseMiddleware<ExceptionHandlingMiddleware>();

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

//app.MapControllers();

app.MapApplicationEndpoints();

app.MapApplicationHubs();


if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(p => p.Path = "/swagger/{documentName}/swagger.yaml");

    app.UseSwaggerUi3(options =>
    {
        var descriptions = app.DescribeApiVersions();

        // build a swagger endpoint for each discovered API version
        foreach (var description in descriptions)
        {
            var name = $"v{description.ApiVersion}";
            var url = $"/swagger/v{GetApiVersion(description)}/swagger.yaml";

            options.SwaggerRoutes.Add(new SwaggerUi3Route(name, url));
        }
    });

    static string GetApiVersion(Asp.Versioning.ApiExplorer.ApiVersionDescription description)
    {
        var apiVersion = description.ApiVersion;
        return (apiVersion.MinorVersion == 0
            ? apiVersion.MajorVersion.ToString()
            : apiVersion.ToString())!;
    }
}

app.UseRateLimiter();

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var dbProviderName = context.Database.ProviderName;

    if (dbProviderName!.Contains("SqlServer"))
    {
        //await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        try
        {
            await ApplyMigrations(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred when applying migrations to the " +
                "database. Error: {Message}", ex.Message);
        }
    }

    if (args.Contains("--seed"))
    {
        try
        {
            await Seed.SeedData(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the " +
                "database. Error: {Message}", ex.Message);
        }

        return;
    }
}

app.Run();

static async Task ApplyMigrations(ApplicationDbContext context)
{
    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
    if (pendingMigrations.Count() > 0)
    {
        await context.Database.MigrateAsync();
    }
}

// INFO: Makes Program class visible to IntegrationTests.
public partial class Program { }