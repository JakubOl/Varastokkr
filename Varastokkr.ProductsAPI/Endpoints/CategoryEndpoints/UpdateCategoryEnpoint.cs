namespace Varastokkr.ProductsAPI.Endpoints.CategoryEndpoints;

internal class UpdateProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("categories/{id:Guid}",
                async (Guid id, 
                    CreateUpdateCategoryDto dto,
                    ILogger<GetCategoriesEndpoint> logger,
                    ProductDbContext db) =>
                {
                    var category = await db.Categories.FirstOrDefaultAsync(p => p.Id == id);
                    if (category == null)
                        return Results.NotFound($"Category with id: {id} does not exist.");

                    var categoryWithNameExists = await db.Categories.AnyAsync(p => p.Name == dto.Name && p.Id != category.Id);
                    if (categoryWithNameExists)
                        return Results.BadRequest($"Category with name: {dto.Name} already exists.");

                    category.Name = dto.Name;
                    category.Description = dto.Description;
                    category.UpdatedAt = DateTime.UtcNow;

                    await db.SaveChangesAsync();

                    // Send event && Update cache

                    return Results.Ok(category);
                })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("UpdateCategory")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Update Category endpoint";
                operation.Description = "Updates category.";
                return operation;
            });
    }
}
