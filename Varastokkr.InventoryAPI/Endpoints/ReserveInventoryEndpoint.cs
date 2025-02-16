using Microsoft.EntityFrameworkCore;
using Varastokkr.InventoryAPI.Dtos;
using Varastokkr.InventoryAPI.Entities;
using Varastokkr.InventoryAPI.Enums;
using Varastokkr.InventoryAPI.Infrastructure;
using Varastokkr.Shared.Abstract;

namespace Varastokkr.InventoryAPI.Endpoints;

internal class ReserveInventoryEndpoint : IEndpoint
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

                    if (inventory.AvailableQuantity - dto.Quantity <= 0)
                        return Results.BadRequest($"Not enough available quantity. Available: {inventory.AvailableQuantity}.");

                    inventory.ReservedQuantity += dto.Quantity;
                    inventory.LastUpdated = DateTime.UtcNow;

                    // Event?
                    var transaction = new InventoryTransaction
                    {
                        InventoryId = inventory.Id,
                        TransactionType = InventoryTransactionType.Reservation,
                        Quantity = dto.Quantity,
                        TransactionDate = DateTime.UtcNow,
                        Comment = "Inventory reserved"
                    };

                    await db.InventoryTransactions.AddAsync(transaction);
                    await db.SaveChangesAsync();

                    return Results.Ok("Inventory reserved successfully.");
                })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("ReserveProductInventory")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Reserve product inventory endpoint";
                operation.Description = "Reserves inventory for product.";
                return operation;
            });
    }
}
