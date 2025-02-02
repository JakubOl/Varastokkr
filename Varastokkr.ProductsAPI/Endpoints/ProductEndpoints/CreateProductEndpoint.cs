using Varastokkr.ProductsAPI.Entities;

namespace Varastokkr.ProductsAPI.Endpoints.ProductEndpoints;

internal class CreateCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("products",
                async (CreateUpdateProductDto dto,
                ILogger<GetCategoriesEndpoint> logger,
                ProductDbContext db) =>
                {
                    var product = new Product
                    {
                        Id = Guid.NewGuid(),
                        Sku = dto.Sku,
                        Name = dto.Name,
                        Description = dto.Description,
                        Price = dto.Price,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };

                    var result = await db.Products.AddAsync(product);
                    await db.SaveChangesAsync();

                    return Results.Created($"/products/{product.Id}", result);
                })
            .Produces(StatusCodes.Status201Created)
            //.Produces(StatusCodes.Status400BadRequest)
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
