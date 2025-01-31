namespace Varastokkr.ProductsAPI.Endpoints;

internal class GetProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products/{id}",
                async (Guid id,
                ILogger<GetProductsEndpoint> logger,
                ProductDbContext db) =>
                {
                    var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);

                    var productDto = product.MapToDto();

                    return Results.Ok(productDto);
                })
            .Produces(StatusCodes.Status200OK)
            //.Produces(StatusCodes.Status400BadRequest)
            //.Produces(StatusCodes.Status401Unauthorized)
            .WithName("GetProduct")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Product endpoint";
                operation.Description = "Gets product from db.";
                return operation;
            });
    }
}
