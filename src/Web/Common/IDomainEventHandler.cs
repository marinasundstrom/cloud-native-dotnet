using MediatR;
using BlazorApp1.Domain;

namespace BlazorApp1.Common;

public interface IDomainEventHandler<TDomainEvent>
    : INotificationHandler<TDomainEvent>
    where TDomainEvent : DomainEvent
{

}