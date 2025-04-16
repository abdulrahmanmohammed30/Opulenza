using Opulenza.Domain.Entities.Categories;

namespace Opulenza.Application.Common.interfaces;

public interface ICategoryImageRepository : IRepository<CategoryImage>
{
    Task<List<CategoryImage>> GetImagesByCategoryId(int categoryId, CancellationToken cancellationToken);

    void AddImages(IEnumerable<CategoryImage> categoryImages);
}
