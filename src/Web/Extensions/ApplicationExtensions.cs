using BlazorApp1.Domain;
using BlazorApp1.Extensions;
using BlazorApp1.Infrastructure;

namespace BlazorApp1.Web.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddUniverse(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPresentation()
            .AddApplication()
            .AddDomain()
            .AddInfrastructure(configuration);

        return services;
    }
}
