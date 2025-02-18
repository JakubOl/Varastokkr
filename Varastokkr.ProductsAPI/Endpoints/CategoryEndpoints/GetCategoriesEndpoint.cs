﻿using Varastokkr.Shared.Models;

namespace Varastokkr.ProductAPI.Endpoints.CategoryEndpoints;

internal class GetCategoriesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories",
                async ([AsParameters] QueryParametersDto dto,
                    ILogger<GetCategoriesEndpoint> logger,
                    ProductDbContext db) =>
                {
                    var query = db.Categories
                        .AsQueryable();

                    if (!string.IsNullOrWhiteSpace(dto.SearchPhrase))
                    {
                        query = query.Where(p => p.Name.Contains(dto.SearchPhrase) 
                            || p.Description.Contains(dto.SearchPhrase));
                    }

                    var categoriesCount = await query.CountAsync();

                    var categories = await query
                        .OrderBy(p => p.Name)
                        .Skip((dto.Page - 1) * dto.PageSize)
                        .Take(dto.PageSize)
                        .AsNoTracking()
                        .ToListAsync();

                    var response = new PaginatedResult<Category>(categories, categoriesCount);

                    return Results.Ok(response);
                })
            .Produces(StatusCodes.Status200OK)
            .WithName("GetCategories")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Categories endpoint";
                operation.Description = "Gets categories from db.";
                return operation;
            });
    }
}
