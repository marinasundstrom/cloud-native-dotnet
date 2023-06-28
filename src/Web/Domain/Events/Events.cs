using BlazorApp1.Domain.ValueObjects;

namespace BlazorApp1.Domain.Events;

public sealed record UserCreated(UserId ChannelId) : DomainEvent;
