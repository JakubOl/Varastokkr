namespace Varastokkr.ProductsAPI.Endpoints.CategoryEndpoints;

internal class DeleteCategoryEnpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("categories/{id:Guid}",
                async (Guid id,
                    ILogger<GetCategoriesEndpoint> logger,
                    ProductDbContext db) =>
                {
                    var category = await db.Categories.FirstOrDefaultAsync(p => p.Id == id);

                    if (category == null)
                        return Results.BadRequest($"Category with id: {id} does not exist.");

                    var productsWithCategoryCount = await db.Products.CountAsync(p => p.CategoryId = id);

                    if (productsWithCategoryCount > 0)
                        return Results.BadRequest($"Category {category.Name} can't be deleted. There are {productsWithCategoryCount} products with this category.");

                    db.Categories.Remove(category);
                    db.SaveChanges();

                    return Results.Ok(new { message = "Category deleted successfully!" });
                })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("DeleteCategory")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Delete category endpoint";
                operation.Description = "Deletes category from db.";
                return operation;
            });
    }
}
