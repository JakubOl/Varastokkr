using Bogus;
using Varastokkr.Shared;

namespace Varastokkr.IdentityAPI.Infrastructure;

internal class DbSeed(ILogger<DbSeed> logger) : IDbSeeder<ProductDbContext>
{
    public async Task SeedAsync(ProductDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        await SeedCategories(context);
        await SeedProducts(context);
    }

    private async Task SeedCategories(ProductDbContext context)
    {
        var hasAnyCategory = await context.Categories.AnyAsync();

        if (hasAnyCategory)
        {
            logger.LogInformation("Categories already created. Skipping seed.");
            return;
        }

        var categoryFaker = new Faker<Category>()
            .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
            .RuleFor(c => c.Description, f => f.Lorem.Sentence(5))
            .RuleFor(c => c.CreatedAt, f => f.Date.Past(2));

        var categories = categoryFaker.Generate(10);

        categories = categories
            .GroupBy(c => c.Name)
            .Select(g => g.First())
            .ToList();

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }

    private async Task SeedProducts(ProductDbContext context)
    {
        var hasAnyProduct = await context.Products.AnyAsync();

        if (hasAnyProduct)
        {
            logger.LogInformation("Products already created. Skipping seed.");
            return;
        }

        var categories = context.Categories.ToList();

        var productFaker = new Faker<Product>()
            .RuleFor(p => p.Sku, f => f.Random.Replace("*********"))
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(10, 500)))
            .RuleFor(p => p.CreatedAt, f => f.Date.Past(1))
            .RuleFor(p => p.CategoryId, f => f.PickRandom(categories).Id);

        var products = productFaker.Generate(50);

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}