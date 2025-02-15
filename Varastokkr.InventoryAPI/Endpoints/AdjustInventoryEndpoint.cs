using Microsoft.EntityFrameworkCore;
using Varastokkr.InventoryAPI.Dtos;
using Varastokkr.InventoryAPI.Entities;
using Varastokkr.InventoryAPI.Enums;
using Varastokkr.InventoryAPI.Infrastructure;
using Varastokkr.Shared.Abstract;

namespace Varastokkr.InventoryAPI.Endpoints;

internal class AdjustInventoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("inventory/{id:Guid}/adjust",
                async (Guid id,
                    InventoryDto dto,
                    ILogger<GetInventoryEnpoint> logger,
                    InventoryDbContext db) =>
                {
                    if (dto.OnHandQuantity < 0)
                        return Results.BadRequest($"Quantity cannot be less than 0.");

                    var inventory = await db.Inventories.FirstOrDefaultAsync(i => i.ProductId == id);
                    if (inventory == null)
                        return Results.NotFound($"Inventory for product with id: {id} does not exist.");

                    inventory.OnHandQuantity = dto.OnHandQuantity;
                    inventory.LastUpdated = DateTime.UtcNow;

                    var transaction = new InventoryTransaction
                    {
                        InventoryId = inventory.Id,
                        TransactionType = InventoryTransactionType.Adjustment,
                        Quantity = dto.OnHandQuantity,
                        TransactionDate = DateTime.UtcNow,
                        Comment = "Inventory adjusted"
                    };

                    await db.InventoryTransactions.AddAsync(transaction);
                    await db.SaveChangesAsync();

                    return Results.Ok("Inventory adjusted successfully.");
                })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("AdjustProductInventory")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Adjust product inventory endpoint";
                operation.Description = "Adjusts inventory for product.";
                return operation;
            });
    }
}
