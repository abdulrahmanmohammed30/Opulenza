using Opulenza.Application.Features.Products.Queries.Common;
using Opulenza.Domain.Entities.Categories;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Application.Common.interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task AddCategoriesToProductWithId(int productId, IEnumerable<Category> categories, CancellationToken cancellationToken = default);
    Task<string?> GetLastSlugWithName(string slug, CancellationToken cancellationToken = default);
    Task<ProductResult?> GetProductWithId(int id, CancellationToken cancellationToken = default);
    Task<ProductResult?> GetProductWithSlug(string slug, CancellationToken cancellationToken = default);

}