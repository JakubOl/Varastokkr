using Microsoft.EntityFrameworkCore;
using Varastokkr.InventoryAPI.Infrastructure;
using Varastokkr.Shared.Abstract;

namespace Varastokkr.InventoryAPI.Endpoints;

internal class GetInventoryTransactionsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inventory/{id:Guid}/transactions",
                async (Guid id,
                    ILogger<GetInventoryEnpoint> logger,
                    InventoryDbContext db) =>
                {
                    var inventoryTransactions = await db.InventoryTransactions
                         .Where(t => t.InventoryId == id)
                         .OrderByDescending(t => t.TransactionDate)
                         .AsNoTracking()
                         .ToListAsync();

                    return Results.Ok(inventoryTransactions);
                })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetProductInventoryTransactions")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Product inventory transactions endpoint";
                operation.Description = "Gets inventory transactions for product from db.";
                return operation;
            });
    }
}

