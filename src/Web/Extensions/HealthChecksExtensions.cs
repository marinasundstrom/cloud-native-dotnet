using BlazorApp1.Infrastructure.Persistence;

namespace BlazorApp1.Web.Extensions;

public static class HealthChecksExtensions
{
    public static IServiceCollection AddHealthChecksServices(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        return services;
    }
}
