namespace Varastokkr.ProductsAPI.Endpoints.ProductEndpoints;

internal class UpdateProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("products/{id:Guid}",
                async (Guid id, 
                    CreateUpdateProductDto dto,
                    ILogger<GetCategoriesEndpoint> logger,
                    ProductDbContext db) =>
                {
                    var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
                    if (product == null)
                        return Results.NotFound($"Product with id: {id} does not exist.");

                    var productWithNameExists = await db.Products.AnyAsync(p => p.Name == dto.Name && p.Id != product.Id);
                    if (productWithNameExists)
                        return Results.BadRequest($"Product with name: {dto.Name} already exists.");

                    product.Sku = dto.Sku;
                    product.Name = dto.Name;
                    product.Description = dto.Description;
                    product.Price = dto.Price;
                    product.UpdatedAt = DateTime.UtcNow;

                    await db.SaveChangesAsync();

                    // Send event && Update cache

                    return Results.Ok(product);
                })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("UpdateProduct")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Update Product endpoint";
                operation.Description = "Updates product.";
                return operation;
            });
    }
}
