namespace Varastokkr.ProductsAPI.Dtos
{
    internal record ProductsDto(IEnumerable<ProductDto> Products, int Count);
    internal record ProductDto(Guid Id, string Sku, string Name, string Description, decimal Price, string CategoryName, DateTime CreatedAt, DateTime UpdatedAt);
    internal record GetProductsDto(string SearchPhrase, int Page, int PageSize);
    internal record CreateUpdateProductDto(string Sku, string Name, string Description, decimal Price, Guid CategoryId);
}
