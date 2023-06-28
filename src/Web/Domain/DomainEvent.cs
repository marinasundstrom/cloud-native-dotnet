using BlazorApp1.Domain.ValueObjects;
using MediatR;

namespace BlazorApp1.Domain;

public abstract record DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();

    public DateTime Timestamp { get; } = DateTime.UtcNow;

    public UserId? CurrentUserId { get; set; } 
}