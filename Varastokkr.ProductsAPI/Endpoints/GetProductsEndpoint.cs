namespace Varastokkr.ProductsAPI.Endpoints;

internal class GetProductsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products",
                async (GetProductsDto dto,
                ILogger<GetProductsEndpoint> logger,
                ProductDbContext db) =>
                {
                    var productsCount = await db.Products.CountAsync();

                    var products = await db.Products
                        .Skip(dto.Skip)
                        .Take(dto.Take)
                        .ToListAsync();

                    var response = new ProductsDto(products.Select(x => x.MapToDto()), productsCount);

                    return Results.Ok(response);
                })
            .Produces(StatusCodes.Status200OK)
            //.Produces(StatusCodes.Status400BadRequest)
            //.Produces(StatusCodes.Status401Unauthorized)
            .WithName("GetProducts")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Products endpoint";
                operation.Description = "Gets products from db.";
                return operation;
            });
    }
}
