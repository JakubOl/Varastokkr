namespace Varastokkr.ProductsAPI.Dtos
{
    internal record GetProductsRequest(int Skip, int Take);
    internal record CreateUpdateProductRequestDto(Guid Id);
}
