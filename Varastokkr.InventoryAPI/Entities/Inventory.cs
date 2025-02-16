using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Varastokkr.InventoryAPI.Entities;

internal class Inventory
{
    [Key]
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int WarehouseId { get; set; }
    public int OnHandQuantity { get; set; }
    public int ReservedQuantity { get; set; }
    public int ReorderThreshold { get; set; }
    public DateTime LastUpdated { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }
    public ICollection<InventoryTransaction> Transactions { get; set; } = new List<InventoryTransaction>();

    [NotMapped]
    public int AvailableQuantity => OnHandQuantity - ReservedQuantity;
}
