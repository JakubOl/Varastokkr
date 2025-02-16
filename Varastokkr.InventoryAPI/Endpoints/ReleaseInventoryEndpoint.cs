using Microsoft.EntityFrameworkCore;
using Varastokkr.InventoryAPI.Dtos;
using Varastokkr.InventoryAPI.Entities;
using Varastokkr.InventoryAPI.Enums;
using Varastokkr.InventoryAPI.Infrastructure;
using Varastokkr.Shared.Abstract;

namespace Varastokkr.InventoryAPI.Endpoints;

internal class ReleaseInventoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("inventory/{id:Guid}/reserve",
                async (Guid id,
                    InventoryDto dto,
                    ILogger<GetInventoryEnpoint> logger,
                    InventoryDbContext db) =>
                {
                    if (dto.Quantity < 0)
                        return Results.BadRequest($"Quantity cannot be less than 0.");

                    var inventory = await db.Inventories.FirstOrDefaultAsync(i => i.ProductId == id);
                    if (inventory == null)
                        return Results.NotFound($"Inventory for product with id: {id} does not exist.");

                    inventory.ReservedQuantity -= dto.Quantity;
                    inventory.LastUpdated = DateTime.UtcNow;

                    // Event?
                    var transaction = new InventoryTransaction
                    {
                        InventoryId = inventory.Id,
                        TransactionType = InventoryTransactionType.Release,
                        Quantity = dto.Quantity,
                        TransactionDate = DateTime.UtcNow,
                        Comment = "Reserved inventory released"
                    };

                    await db.InventoryTransactions.AddAsync(transaction);
                    await db.SaveChangesAsync();

                    return Results.Ok("Inventory relesead successfully.");
                })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("ReleaseProductInventory")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Release product inventory endpoint";
                operation.Description = "Releases inventory for product.";
                return operation;
            });
    }
}