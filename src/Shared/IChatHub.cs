namespace BlazorApp1.Features.Chat;

public interface IChatHub
{
    Task<Guid> PostMessage(Guid channelId, Guid? replyTo, string content);
}
