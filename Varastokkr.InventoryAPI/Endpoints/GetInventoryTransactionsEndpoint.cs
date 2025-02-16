using Microsoft.EntityFrameworkCore;
using Varastokkr.InventoryAPI.Infrastructure;
using Varastokkr.Shared.Abstract;
using Varastokkr.Shared.Models;

namespace Varastokkr.InventoryAPI.Endpoints;

internal class GetInventoryTransactionsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inventory/{id:Guid}/transactions",
                async (Guid id,
                    [AsParameters] QueryParametersDto dto,
                    ILogger <GetInventoryEnpoint> logger,
                    InventoryDbContext db) =>
                {
                    var inventoryTransactions = await db.InventoryTransactions
                        .Where(t => t.InventoryId == id 
                            && (string.IsNullOrEmpty(dto.SearchPhrase) || t.Comment.Contains(dto.SearchPhrase, StringComparison.InvariantCultureIgnoreCase)))
                        .OrderByDescending(t => t.TransactionDate)
                        .Skip((dto.Page - 1) * dto.PageSize)
                        .Take(dto.PageSize)
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

