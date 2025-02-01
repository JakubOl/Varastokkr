namespace Varastokkr.ProductsAPI.Dtos
{
    internal record GetProductsDto(int Skip, int Take);
    internal record CreateUpdateProductDto(string Sku, string Name, string Description, decimal Price);
}
