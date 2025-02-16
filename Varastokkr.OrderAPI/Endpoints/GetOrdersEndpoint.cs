using Microsoft.EntityFrameworkCore;
using Varastokkr.OrderAPI.Entities;
using Varastokkr.OrderAPI.Infrastructure;
using Varastokkr.Shared.Abstract;
using Varastokkr.Shared.Models;

namespace Varastokkr.OrderAPI.Endpoints;

internal class GetOrdersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("orders",
                async ([AsParameters] QueryParametersDto dto,
                    ILogger<GetOrdersEndpoint> logger,
                    OrderDbContext db) =>
                {
                    var query = db.Orders
                        .AsQueryable();

                    var count = await query.CountAsync();

                    var orders = await query
                        .Include(x => x.OrderItems)
                        .OrderByDescending(p => p.OrderDate)
                        .Skip((dto.Page - 1) * dto.PageSize)
                        .Take(dto.PageSize)
                        .AsNoTracking()
                        .ToListAsync();

                    var response = new PaginatedResult<Order>(orders, count);

                    return Results.Ok(response);
                })
            .Produces(StatusCodes.Status200OK)
            .WithName("GetOrders")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Orders endpoint";
                operation.Description = "Gets orders from db.";
                return operation;
            });
    }
}
