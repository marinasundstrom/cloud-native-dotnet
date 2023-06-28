using Bunit;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using NSubstitute;
using BlazorApp1;
using BlazorApp1.Pages;

namespace BlazorApp1.Tests;

public class ChannelPageTest
{
    [Fact]
    public void MessagesShouldLoadOnInitializedSuccessful()
    {
        // Arrange
        using var ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        ctx.Services.AddMudServices();
        ctx.Services.AddLocalization();

        var fakeThemeManager = Substitute.For<BlazorApp1.Theming.IThemeManager>();
        ctx.Services.AddSingleton(fakeThemeManager);

        var fakeAccessTokenProvider = Substitute.For<BlazorApp1.Services.IAccessTokenProvider>();
        ctx.Services.AddSingleton(fakeAccessTokenProvider);

        var fakeCurrentUserService = Substitute.For<BlazorApp1.Services.ICurrentUserService>();
        fakeCurrentUserService.GetUserIdAsync().Returns("foo");
        ctx.Services.AddSingleton(fakeCurrentUserService);

        var fakeTimeViewService = Substitute.For<BlazorApp1.Chat.Messages.ITimeViewService>();
        ctx.Services.AddSingleton(fakeTimeViewService);

        var fakeMessagesClient = Substitute.For<IMessagesClient>();
        fakeMessagesClient.GetMessagesAsync(Arg.Any<Guid>(), null, null, null, default)
            .ReturnsForAnyArgs(t => new ItemsResultOfMessage()
            {
                Items = new[]
                {
                    new Message
                    {
                        Id = Guid.NewGuid(),
                        Content = "Hello world",
                        Published = DateTimeOffset.Now.AddMinutes(-3),
                        PublishedBy = new User {
                            Id = "1",
                            Name = "Foo"
                        }
                    },
                },
                TotalItems = 3
            });

        ctx.Services.AddSingleton<IMessagesClient>(fakeMessagesClient);

        var fakeUsersClient = Substitute.For<IUsersClient>();
        fakeUsersClient.GetUserInfoAsync()
            .ReturnsForAnyArgs(t => new UserInfo
            {
                Id = "1",
                Name = "Foo"
            });

        ctx.Services.AddSingleton<IUsersClient>(fakeUsersClient);

        var cut = ctx.RenderComponent<BlazorApp1.Chat.Channels.ChannelPage>();

        // Act
        //cut.Find("button").Click();

        // Assert
        cut.WaitForState(() => cut.Find("div.message") != null);
    }
}
