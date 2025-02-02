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
                        return Results.BadRequest($"Category with id: {id} does not exist.");

                    category.Name = dto.Name;
                    category.Description = dto.Description;
                    category.UpdatedAt = DateTime.UtcNow;

                    await db.SaveChangesAsync();

                    return Results.Ok(category);
                })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("UpdateCategory")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Update Category endpoint";
                operation.Description = "Updates category.";
                return operation;
            });
    }
}
