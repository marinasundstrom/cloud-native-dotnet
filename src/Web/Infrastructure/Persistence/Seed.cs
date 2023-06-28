using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedData(ApplicationDbContext context)
    {
        await context.SaveChangesAsync();
    }
}