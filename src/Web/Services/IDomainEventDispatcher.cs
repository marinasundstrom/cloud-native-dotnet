﻿using BlazorApp1.Domain;

namespace BlazorApp1.Services;

public interface IDomainEventDispatcher
{
    Task Dispatch(DomainEvent domainEvent, CancellationToken cancellationToken = default);
}
