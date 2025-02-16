using System.ComponentModel.DataAnnotations;
using Varastokkr.OrderAPI.Enums;

namespace Varastokkr.OrderAPI.Entities;

internal class Order
{
    [Key]
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; set; }
    public int CustomerId { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = [];
}
