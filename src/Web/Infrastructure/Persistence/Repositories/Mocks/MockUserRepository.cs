using BlazorApp1.Domain.Specifications;
using BlazorApp1.Domain.ValueObjects;

namespace BlazorApp1.Infrastructure.Persistence.Repositories.Mocks;

public sealed class MockUserRepository : IUserRepository
{
    private readonly MockUnitOfWork mockUnitOfWork;

    public MockUserRepository(MockUnitOfWork mockUnitOfWork)
    {
        this.mockUnitOfWork = mockUnitOfWork;
    }

    public void Add(User item)
    {
        mockUnitOfWork.Items.Add(item);
        mockUnitOfWork.NewItems.Add(item);
    }

    public void Dispose()
    {
        foreach (var item in mockUnitOfWork.NewItems)
        {
            mockUnitOfWork.Items.Remove(item);
        }
    }

    public Task<User?> FindByIdAsync(UserId id, CancellationToken cancellationToken = default)
    {
        var item = mockUnitOfWork.Items
            .OfType<User>()
            .FirstOrDefault(x => x.Id.Equals(id));

        return Task.FromResult(item);
    }

    public IQueryable<User> GetAll()
    {
        return mockUnitOfWork.Items
            .OfType<User>()
            .AsQueryable();
    }

    public IQueryable<User> GetAll(ISpecification<User> specification)
    {
        return mockUnitOfWork.Items
            .OfType<User>()
            .AsQueryable()
            .Where(specification.Criteria);
    }

    public void Remove(User item)
    {
        mockUnitOfWork.Items.Remove(item);
    }
}

