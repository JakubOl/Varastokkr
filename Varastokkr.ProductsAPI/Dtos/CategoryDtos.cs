namespace Varastokkr.CategorysAPI.Dtos;

internal record CategoriesDto(IEnumerable<Category> Categories, int Count);
internal record GetCategoriesDto(string SearchPhrase, int Page, int PageSize);
internal record CreateUpdateCategoryDto(string Name, string Description);
