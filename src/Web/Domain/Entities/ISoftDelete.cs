using BlazorApp1.Domain.ValueObjects;

namespace BlazorApp1.Domain.Entities;

public interface ISoftDelete
{
    UserId? DeletedById { get; set; }
    DateTimeOffset? Deleted { get; set; }
}
