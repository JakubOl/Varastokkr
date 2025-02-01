namespace Varastokkr.ProductsAPI.Endpoints;

internal class DeleteProductEnpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("products/{id:Guid}",
                async (Guid id,
                ILogger<GetProductsEndpoint> logger,
                ProductDbContext db) =>
                {
                    var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);

                    if (product == null)
                        return Results.BadRequest($"Product with id: {id} does not exist.");

                    db.Products.Remove(product);
                    db.SaveChanges();

                    return Results.Ok(new { message = "Product deleted successfully!" });
                })
            .Produces(StatusCodes.Status200OK)
            //.Produces(StatusCodes.Status400BadRequest)
            //.Produces(StatusCodes.Status401Unauthorized)
            .WithName("DeleteProduct")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Delete product endpoint";
                operation.Description = "Deletes product from db.";
                return operation;
            });
    }
}
