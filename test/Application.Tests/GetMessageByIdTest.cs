﻿using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using BlazorApp1.Services;
using BlazorApp1.Infrastructure.Persistence;
using BlazorApp1.Infrastructure.Persistence.Repositories;
using BlazorApp1.Domain;
using System;

namespace BlazorApp1.Features.Chat.Messages;


public class GetTodoTest
{
    [Fact]
    public async Task GetMessageById_MessageNotFound()
    {
        // Arrange

        var fakeDomainEventDispatcher = Substitute.For<IDomainEventDispatcher>();

        using (var connection = new SqliteConnection("Data Source=:memory:"))
        {
            connection.Open();

            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var unitOfWork = new ApplicationDbContext(dbContextOptions);

            await unitOfWork.Database.EnsureCreatedAsync();

            var todoRepository = new MessageRepository(unitOfWork);

            var commandHandler = new GetMessageById.Handler(todoRepository, new DtoComposer(unitOfWork, new DtoFactory()));

            Guid nonExistentMessageId = Guid.NewGuid();

            // Act

            var getMessageByIdCommand = new GetMessageById(nonExistentMessageId);

            var result = await commandHandler.Handle(getMessageByIdCommand, default);

            // Assert

            Assert.True(result.HasError(Errors.Messages.MessageNotFound));
        }
    }
}