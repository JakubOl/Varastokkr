namespace Varastokkr.ProductsAPI.Dtos
{
    internal record ProductsDto(IEnumerable<ProductDto> Products, int Count);
    internal record ProductDto(Guid Id, string Sku, string Name, string Description, decimal Price, DateTime CreatedAt, DateTime UpdatedAt);
}