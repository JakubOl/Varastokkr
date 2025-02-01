namespace Varastokkr.ProductsAPI.Dtos
{
    internal record GetProductsRequest(int Skip, int Take);
    internal record CreateUpdateProductRequestDto(string Sku, string Name, string Description, decimal Price);
}
