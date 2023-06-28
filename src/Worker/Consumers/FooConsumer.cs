using BlazorApp1.Contracts;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;

namespace Worker.Consumers;

public sealed class FooConsumer : IConsumer<Foo>
{
    readonly IDistributedCache _distributedCache;

    public FooConsumer(IDistributedCache distributedCache) 
    {
        _distributedCache = distributedCache;
    }

    public async Task Consume(ConsumeContext<Foo> context)
    {
        Console.WriteLine($"Foo: {context.Message.Text}");

        await _distributedCache.SetStringAsync("result-text2", context.Message.Text, context.CancellationToken);

        await context.RespondAsync(new FooResponse($"OK: {context.Message.Text}"));
    }
}
