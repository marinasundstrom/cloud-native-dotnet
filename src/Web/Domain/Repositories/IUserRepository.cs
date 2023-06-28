using BlazorApp1.Domain.ValueObjects;

namespace BlazorApp1.Domain.Repositories;

public interface IUserRepository : IRepository<User, UserId>
{
}