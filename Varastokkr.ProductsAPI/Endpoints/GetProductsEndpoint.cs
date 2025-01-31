namespace Varastokkr.ProductsAPI.Endpoints;

internal class GetProductsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products",
                async (GetProductsRequest dto,
                ILogger<GetProductsEndpoint> logger,
                ProductDbContext db) =>
                {
                    var productsCount = await db.Products.LongCountAsync();

                    var products = await db.Products
                        .Skip(dto.Skip)
                        .Take(dto.Take)
                        .ToListAsync();

                    var productsDtos = products.Select(x => x.MapToDto());

                    return Results.Ok(productsDtos);
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
