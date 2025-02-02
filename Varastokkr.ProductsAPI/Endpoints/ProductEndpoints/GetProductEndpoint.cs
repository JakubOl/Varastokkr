namespace Varastokkr.ProductsAPI.Endpoints.ProductEndpoints;

internal class GetCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products/{id:Guid}",
                async (Guid id,
                ILogger<GetCategoriesEndpoint> logger,
                ProductDbContext db) =>
                {
                    var product = await db.Products
                        .Include(x => x.Category)
                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (product == null)
                        return Results.BadRequest($"Product with id: {id} does not exist.");

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
