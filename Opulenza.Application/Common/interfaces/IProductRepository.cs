using Opulenza.Domain.Entities.Categories;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Application.Common.interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task AddCategoriesToProductWithId(int productId, IEnumerable<Category> categories);
    Task<string?> GetLastSlugWithName(string slug);

}