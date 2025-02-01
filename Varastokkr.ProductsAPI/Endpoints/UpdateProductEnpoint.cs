namespace Varastokkr.ProductsAPI.Endpoints;

internal class UpdateProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("products/{id:Guid}",
                async (Guid id, 
                CreateUpdateProductRequestDto dto,
                ILogger<GetProductsEndpoint> logger,
                ProductDbContext db) =>
                {
                    var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);

                    if (product == null)
                        return Results.BadRequest($"Product with id: {id} does not exist.");

                    product.Sku = dto.Sku;
                    product.Name = dto.Name;
                    product.Description = dto.Description;
                    product.Price = dto.Price;
                    product.UpdatedAt = DateTime.UtcNow;

                    await db.SaveChangesAsync();

                    return Results.Ok(product);
                })
            .Produces(StatusCodes.Status200OK)
            //.Produces(StatusCodes.Status400BadRequest)
            //.Produces(StatusCodes.Status401Unauthorized)
            .WithName("UpdateProduct")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Update Product endpoint";
                operation.Description = "Updates product.";
                return operation;
            });
    }
}
