using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Requests.Categories;

namespace SoarBeyond.Domain.Providers.Interfaces;

public interface ICategoryProvider
{
    Task<Category> CreateAsync(CreateCategoryRequest request);
    Task<IEnumerable<Category>> GetAllAsync(GetAllCategoriesRequest request);
}