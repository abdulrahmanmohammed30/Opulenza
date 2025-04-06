using Opulenza.Domain.Entities.Categories;

namespace Opulenza.Application.Common.interfaces;

public interface ICategoryRepository: IRepository<Category>
{
     Task<List<Category>> GetCategories(IEnumerable<int> categoryIds);
}