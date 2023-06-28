using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using BlazorApp1.Domain.ValueObjects;

namespace BlazorApp1.Infrastructure.Persistence.ValueConverters;

internal sealed class UserIdConverter : ValueConverter<UserId, string>
{
    public UserIdConverter()
        : base(v => v.Value, v => new(v))
    {
    }
}