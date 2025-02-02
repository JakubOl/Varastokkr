namespace Varastokkr.ProductsAPI.Endpoints.CategoryEndpoints;

internal class CreateCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("categories",
                async (CreateUpdateCategoryDto dto,
                    ILogger<GetCategoriesEndpoint> logger,
                    ProductDbContext db) =>
                {
                    var category = new Category
                    {
                        Name = dto.Name,
                        Description = dto.Description,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };

                    var result = await db.Categories.AddAsync(category);
                    await db.SaveChangesAsync();

                    return Results.Created($"/categories/{category.Id}", result);
                })
            .Produces(StatusCodes.Status201Created)
            //.Produces(StatusCodes.Status400BadRequest)
            //.Produces(StatusCodes.Status401Unauthorized)
            .WithName("CreateCategory")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Create Category endpoint";
                operation.Description = "Creates new category.";
                return operation;
            });
    }
}
