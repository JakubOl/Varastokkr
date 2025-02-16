using Microsoft.EntityFrameworkCore;
using Varastokkr.InventoryAPI.Dtos;
using Varastokkr.InventoryAPI.Entities;
using Varastokkr.InventoryAPI.Enums;
using Varastokkr.InventoryAPI.Infrastructure;
using Varastokkr.Shared.Abstract;

namespace Varastokkr.InventoryAPI.Endpoints;

internal class ShipInventoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("inventory/{id:Guid}/ship",
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

                    if (inventory.ReservedQuantity < dto.Quantity)
                        return Results.BadRequest($"Not enough reserved quantity. Reserved: {inventory.AvailableQuantity}.");

                    inventory.ReservedQuantity -= dto.Quantity;
                    inventory.OnHandQuantity -= dto.Quantity;
                    inventory.LastUpdated = DateTime.UtcNow;

                    // Event?
                    var transaction = new InventoryTransaction
                    {
                        InventoryId = inventory.Id,
                        TransactionType = InventoryTransactionType.Shipment,
                        Quantity = -dto.Quantity,
                        TransactionDate = DateTime.UtcNow,
                        Comment = "Inventory shipped"
                    };

                    await db.InventoryTransactions.AddAsync(transaction);
                    await db.SaveChangesAsync();

                    return Results.Ok("Inventory shipped successfully.");
                })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("ShipProductInventory")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Ship product endpoint";
                operation.Description = "Ships inventory for product.";
                return operation;
            });
    }
}