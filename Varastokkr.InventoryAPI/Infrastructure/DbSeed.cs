using Varastokkr.Shared;

namespace Varastokkr.InventoryAPI.Infrastructure;
internal class DbSeed(ILogger<DbSeed> logger) : IDbSeeder<InventoryDbContext>
{
    public async Task SeedAsync(InventoryDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        
    }
}