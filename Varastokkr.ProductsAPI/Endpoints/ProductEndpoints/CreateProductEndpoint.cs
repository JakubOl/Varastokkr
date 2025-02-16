using Varastokkr.ProductAPI.Entities;

namespace Varastokkr.ProductAPI.Endpoints.ProductEndpoints;

internal class CreateCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("products",
                async (CreateUpdateProductDto dto,
                    ILogger<GetCategoriesEndpoint> logger,
                    ProductDbContext db) =>
                {
                    var productWithNameExists = await db.Products.AnyAsync(p => p.Name == dto.Name);
                    if (productWithNameExists)
                        return Results.BadRequest($"Product with name: {dto.Name} already exists.");

                    if (dto.Price <= 0)
                        return Results.BadRequest($"Invalid product price: {dto.Price}.");

                    var product = new Product
                    {
                        Id = Guid.NewGuid(),
                        Sku = dto.Sku,
                        Name = dto.Name,
                        Description = dto.Description,
                        Price = dto.Price,
                        CategoryId = dto.CategoryId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };

                    var result = await db.Products.AddAsync(product);
                    await db.SaveChangesAsync();

                    // Send event && Update cache

                    return Results.Created($"/products/{product.Id}", result);
                })
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            //.Produces(StatusCodes.Status401Unauthorized)
            .WithName("CreateProduct")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Create Product endpoint";
                operation.Description = "Creates new product.";
                return operation;
            });
    }
}
