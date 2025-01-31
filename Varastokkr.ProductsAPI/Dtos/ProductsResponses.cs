namespace Varastokkr.ProductsAPI.Dtos
{
    internal record ProductsDto(List<ProductDto> Products);
    internal record ProductDto(Guid Id, string Sku, string Name, string Description, decimal Price, DateTime CreatedAt, DateTime UpdatedAt);
}