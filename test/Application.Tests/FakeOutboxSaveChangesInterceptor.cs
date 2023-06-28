using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using BlazorApp1.Infrastructure.Persistence.Outbox;
using System.Threading.Tasks;
using System.Threading;
using BlazorApp1.Domain.Entities;
using BlazorApp1.Services;
using System.Linq;

namespace BlazorApp1.Infrastructure.Persistence.Interceptors;

public sealed class FakeOutboxSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IDomainEventDispatcher domainEventDispatcher;

    public FakeOutboxSaveChangesInterceptor(IDomainEventDispatcher domainEventDispatcher)
    {
        this.domainEventDispatcher = domainEventDispatcher;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var entities = context.ChangeTracker
                        .Entries<IHasDomainEvents>()
                        .Where(e => e.Entity.DomainEvents.Any())
                        .Select(e => e.Entity);

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .OrderBy(e => e.Timestamp)
            .ToList();

        await Task.WhenAll(domainEvents.Select(x => domainEventDispatcher.Dispatch(x)));

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}