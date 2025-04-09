using Opulenza.Application.Features.Products.Common;
using Opulenza.Application.Features.Products.Queries.GetProducts;
using Opulenza.Domain.Entities.Categories;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Application.Common.interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task AddCategoriesToProductWithIdAsync(int productId, IEnumerable<Category> categories,
        CancellationToken cancellationToken = default);

    Task<string?> GetLastSlugWithNameAsync(string slug, CancellationToken cancellationToken = default);
    Task<ProductResult?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductResult?> GetProductBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<GetProductListResult> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int productId, CancellationToken cancellationToken = default);

    Task<Product?> GetProductByIdWithCategoriesAsync(int id, CancellationToken cancellationToken);
}