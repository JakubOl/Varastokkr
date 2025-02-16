using Varastokkr.Shared;

namespace Varastokkr.OrderAPI.Infrastructure;

internal class DbSeed(ILogger<DbSeed> logger) : IDbSeeder<OrderDbContext>
{
    public async Task SeedAsync(OrderDbContext context)
    {
        await context.Database.EnsureCreatedAsync();


    }
}
