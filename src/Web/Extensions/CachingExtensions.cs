using StackExchange.Redis;

namespace BlazorApp1.Web.Extensions;

public static class CachingExtensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();

        IConnectionMultiplexer? connection = null;

        var connectionString = configuration.GetConnectionString("Redis") ?? "redis";
        var c = ConfigurationOptions.Parse(connectionString, true);

        connection = ConnectionMultiplexer.Connect(c);

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            return connection;
        });

        services.AddStackExchangeRedisCache(options =>
        {
            //o.Configuration = builder.Configuration.GetConnectionString("redis");
            options.ConnectionMultiplexerFactory = () =>
            {
                return Task.FromResult(connection!);
            };
        });

        return services;
    }


}
