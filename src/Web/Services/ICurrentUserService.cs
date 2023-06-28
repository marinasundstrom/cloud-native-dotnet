using System.Security.Claims;

namespace BlazorApp1.Services;

public interface ICurrentUserService
{
    string? UserId { get; }

    string? ConnectionId { get; }

    bool IsInRole(string role);
}

public interface ICurrentUserServiceInternal : ICurrentUserService
{
    void SetUser(ClaimsPrincipal user);

    void SetConnectionId(string connectionId);
}
