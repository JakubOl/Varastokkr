using Varastokkr.ProductsAPI.Entities;

namespace Varastokkr.ProductsAPI.Infrastructure;

internal class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
}
