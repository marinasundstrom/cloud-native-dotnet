using System.Data.Common;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NSubstitute;
using BlazorApp1.Services;
using BlazorApp1;
using BlazorApp1.Infrastructure.Persistence;
using BlazorApp1.Infrastructure.Persistence.Interceptors;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BlazorApp1.Infrastructure;

public class MessageFixture : IDisposable
{
    private readonly ICurrentUserService fakeCurrentUserService;
    private readonly TimeProvider fakeDateTimeService;
    private SqliteConnection connection = null!;

    public MessageFixture()
    {
        fakeCurrentUserService = Substitute.For<ICurrentUserService>();
        fakeCurrentUserService.UserId.Returns("foo");

        fakeDateTimeService = Substitute.For<TimeProvider>();
        fakeDateTimeService.Now.Returns(DateTime.UtcNow);
    }

    public ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .AddInterceptors(new AuditableEntitySaveChangesInterceptor(fakeCurrentUserService, fakeDateTimeService), new OutboxSaveChangesInterceptor(fakeCurrentUserService))
           .UseSqlite(GetDbConnection())
           .Options;

        var context = new ApplicationDbContext(options);

        context.Database.EnsureCreated();

        return context;
    }

    private DbConnection GetDbConnection()
    {
        connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        return connection;
    }

    public void Dispose()
    {
        connection.Close();
    }
}