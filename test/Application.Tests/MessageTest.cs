using System;
using System.Linq;
using BlazorApp1.Domain.Entities;
using BlazorApp1.Domain.Enums;
using BlazorApp1.Domain.Events;

namespace BlazorApp1.Tests;

public class MessageTest
{
    [Fact]
    public void CreateMessage()
    {
        var todo = new Message(Guid.NewGuid(), "Test");

        todo.DomainEvents.OfType<MessagePosted>().Should().ContainSingle();
    }
}
