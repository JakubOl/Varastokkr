namespace Varastokkr.ProductsAPI.Endpoints.CategoryEndpoints;

internal class GetCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories/{id:Guid}",
                async (Guid id,
                    ILogger<GetCategoriesEndpoint> logger,
                    ProductDbContext db) =>
                {
                    var category = await db.Categories
                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (category == null)
                        return Results.NotFound($"Category with id: {id} does not exist.");

                    return Results.Ok(category);
                })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetCategory")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Category endpoint";
                operation.Description = "Gets category from db.";
                return operation;
            });
    }
}
