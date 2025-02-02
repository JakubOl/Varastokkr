﻿namespace Varastokkr.ProductsAPI.Endpoints.ProductEndpoints;

internal class GetCategoriesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products",
                async (GetProductsDto dto,
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
                        .Skip((dto.PageSize - 1) * dto.PageSize)
                        .Take(dto.PageSize)
                        .AsNoTracking()
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
