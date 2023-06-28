namespace BlazorApp1.Common;

public sealed record ItemsResult<T>(IEnumerable<T> Items, int TotalItems);