using System.Diagnostics;
using BlazorApp1.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace BlazorApp1.Features.Test;

public static class Endpoints
{
    public static WebApplication MapTestEndpoints(this WebApplication app)
    {
        var test = app.NewVersionedApi("Test");

        var group = test.MapGroup("/v{version:apiVersion}/Test")
            .WithOpenApi()
            .HasApiVersion(1, 0)
            .HasApiVersion(2, 0);

        group.MapPost("/", async Task<IResult> ([FromBody] string text, IDistributedCache distributedCache, IPublishEndpoint publishEndpoint, IRequestClient<Foo> requestClient, CancellationToken cancellationToken) =>
        {
            var result = await requestClient.GetResponse<FooResponse>(new Foo(text));

            await distributedCache.SetStringAsync("result-text", result.Message.Text, cancellationToken);

            app.Logger.LogInformation("This is a log message. This is an object: {User}", new { name = "John Doe" });

            return Results.Ok(result.Message);
        })
        .WithName($"Test_{nameof(Test)}")
        .Produces<string>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .RequireAuthorization();

        return app;
    }
}