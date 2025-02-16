using System.ComponentModel.DataAnnotations;
using Varastokkr.InventoryAPI.Enums;

namespace Varastokkr.InventoryAPI.Entities;

internal class InventoryTransaction
{
    [Key]
    public int Id { get; set; }
    public Guid InventoryId { get; set; }
    public InventoryTransactionType TransactionType { get; set; }
    public int Quantity { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Comment { get; set; }
    public Inventory Inventory { get; set; }
}
