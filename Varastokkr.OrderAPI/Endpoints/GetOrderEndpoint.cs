using Microsoft.EntityFrameworkCore;
using Varastokkr.OrderAPI.Infrastructure;
using Varastokkr.Shared.Abstract;
using Varastokkr.Shared.Models;

namespace Varastokkr.OrderAPI.Endpoints;

internal class GetOrderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("orders/{id:Guid}",
                async (Guid id,
                    ILogger<GetOrderEndpoint> logger,
                    OrderDbContext db) =>
                {
                    var order = db.Orders
                        .Include(x => x.OrderItems)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id);

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
