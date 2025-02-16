using Varastokkr.Shared.Models;

namespace Varastokkr.ProductAPI.Endpoints.ProductEndpoints;

internal class GetCategoriesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products",
                async ([AsParameters] QueryParametersDto dto,
                    ILogger<GetCategoriesEndpoint> logger,
                    ProductDbContext db) =>
                {
                    var query = db.Products
                        .Include(p => p.Category)
                        .AsQueryable();

                    if (!string.IsNullOrWhiteSpace(dto.SearchPhrase))
                    {
                        query = query.Where(p => p.Name.Contains(dto.SearchPhrase) 
                            || p.Description.Contains(dto.SearchPhrase) 
                            || p.Category.Name.Contains(dto.SearchPhrase));
                    }

                    var productsCount = await query.CountAsync();

                    var products = await query
                        .OrderBy(p => p.Name)
                        .Skip((dto.Page - 1) * dto.PageSize)
                        .Take(dto.PageSize)
                        .AsNoTracking()
                        .ToListAsync();

                    var response = new PaginatedResult<ProductDto>(products.Select(x => x.MapToDto()), productsCount);

                    return Results.Ok(response);
                })
            .Produces(StatusCodes.Status200OK)
            .WithName("GetProducts")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Products endpoint";
                operation.Description = "Gets products from db.";
                return operation;
            });
    }
}
