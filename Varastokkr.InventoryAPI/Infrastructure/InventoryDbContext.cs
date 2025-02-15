using Microsoft.EntityFrameworkCore;
using Varastokkr.InventoryAPI.Entities;

namespace Varastokkr.InventoryAPI.Infrastructure;

internal class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
{
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<Inventory>()
            .HasMany(i => i.Transactions)
            .WithOne(t => t.Inventory)
            .HasForeignKey(t => t.InventoryId);
    }
}