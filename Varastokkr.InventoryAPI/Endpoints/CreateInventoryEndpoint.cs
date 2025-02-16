using Microsoft.EntityFrameworkCore;
using Varastokkr.InventoryAPI.Dtos;
using Varastokkr.InventoryAPI.Entities;
using Varastokkr.InventoryAPI.Infrastructure;
using Varastokkr.Shared.Abstract;

namespace Varastokkr.InventoryAPI.Endpoints;

internal class CreateInventoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("inventory/{id:Guid}",
                async (Guid id,
                    InventoryDto dto,
                    ILogger<GetInventoryEnpoint> logger,
                    InventoryDbContext db) =>
                {
                    if (dto.Quantity < 0)
                        return Results.BadRequest($"Quantity cannot be less than 0.");

                    var existing = await db.Inventories.FirstOrDefaultAsync(i => i.ProductId == id);
                    if (existing != null)
                        return Results.BadRequest($"Inventory for product with id: {id} already exists.");

                    var inventory = new Inventory
                    {
                        ProductId = dto.ProductId,
                        OnHandQuantity = dto.Quantity,
                        LastUpdated = DateTime.UtcNow,
                    };

                    await db.Inventories.AddAsync(inventory);
                    await db.SaveChangesAsync();

                    return Results.Created($"/inventory/{inventory.ProductId}", inventory);
                })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("CreateProductInventory")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Product inventory endpoint";
                operation.Description = "Creates inventory for product.";
                return operation;
            });
    }
}
