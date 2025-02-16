namespace Varastokkr.ProductAPI.Dtos
{
    internal record ProductDto(Guid Id, string Sku, string Name, string Description, decimal Price, string CategoryName, DateTime CreatedAt, DateTime UpdatedAt);
    internal record CreateUpdateProductDto(string Sku, string Name, string Description, decimal Price, Guid CategoryId);
}
