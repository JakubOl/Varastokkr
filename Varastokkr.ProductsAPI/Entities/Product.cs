using System.ComponentModel.DataAnnotations.Schema;

namespace Varastokkr.ProductsAPI.Entities;

internal class Product
{
    public Guid Id { get; set; }
    public string Sku { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ProductDto MapToDto() => new(Id, Sku, Name, Description, Price, Category.Name, CreatedAt, UpdatedAt);
}
