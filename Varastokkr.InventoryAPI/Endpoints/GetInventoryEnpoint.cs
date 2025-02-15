using Microsoft.EntityFrameworkCore;
using Varastokkr.InventoryAPI.Infrastructure;
using Varastokkr.Shared.Abstract;

namespace Varastokkr.InventoryAPI.Endpoints;

internal class GetInventoryEnpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inventory/{id:Guid}",
                async (Guid id,
                    ILogger<GetInventoryEnpoint> logger,
                    InventoryDbContext db) =>
                {
                    var inventory = await db.Inventories
                         .Include(i => i.Transactions)
                         .FirstOrDefaultAsync(i => i.ProductId == id);

                    if (inventory == null)
                        return Results.NotFound($"Inventory for product with id: {id} does not exist.");

                    return Results.Ok(inventory);
                })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetProductInventory")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Product inventory endpoint";
                operation.Description = "Gets inventory for product from db.";
                return operation;
            });
    }
}
