using Blazored.LocalStorage;

using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp1.Theming;

public static class ServicesExtensions
{
    public static IServiceCollection AddThemeServices(this IServiceCollection services)
    {
        services.AddScoped<SystemColorSchemeDetector>();
        services.AddScoped<IThemeManager, ThemeManager>();

        return services;
    }
}