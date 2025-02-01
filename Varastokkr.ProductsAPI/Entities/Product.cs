namespace Varastokkr.ProductsAPI.Entities;

internal class Product
{
    public Guid Id { get; set; }
    public string Sku { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ProductDto MapToDto() => new(Id, Sku, Name, Description, Price, CreatedAt, UpdatedAt);
}
