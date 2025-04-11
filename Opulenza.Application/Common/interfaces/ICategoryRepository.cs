using Opulenza.Application.Features.Categories.Queries.GetCategories;
using Opulenza.Domain.Entities.Categories;

namespace Opulenza.Application.Common.interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    Task<string?> GetLastSlugWithNameAsync(string slug, CancellationToken cancellationToken = default);

    Task DeleteImagesAsync(int categoryId, CancellationToken cancellationToken = default);

    Task<List<Category>> GetCategoriesAsync(IEnumerable<int> categoryIds,
        CancellationToken cancellationToken = default);

    Task<List<Category>> GetCategoriesAsync(GetCategoriesQuery query,
        CancellationToken cancellationToken = default);

    Task<int> GetCategoriesCountAsync(GetCategoriesQuery query,
        CancellationToken cancellationToken = default);

    Task<bool> HasAncestorCategory(int categoryId, int parentId,
        CancellationToken cancellationToken = default);
}