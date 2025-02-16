namespace Varastokkr.ProductAPI.Endpoints.CategoryEndpoints;

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
                        return Results.NotFound($"Category with id: {id} does not exist.");

                    var productsWithCategoryExists = await db.Products.AnyAsync(p => p.CategoryId == id);
                    if (productsWithCategoryExists)
                        return Results.BadRequest($"Category {category.Name} can't be deleted. There are products with this category.");

                    db.Categories.Remove(category);
                    await db.SaveChangesAsync();

                    // Send event && Update cache

                    return Results.Ok(new { message = "Category deleted successfully!" });
                })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("DeleteCategory")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Delete category endpoint";
                operation.Description = "Deletes category from db.";
                return operation;
            });
    }
}
