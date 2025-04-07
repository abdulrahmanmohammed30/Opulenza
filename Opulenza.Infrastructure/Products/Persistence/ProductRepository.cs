using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Products.Queries.Common;
using Opulenza.Application.Features.Products.Queries.GetProducts;
using Opulenza.Domain.Entities.Categories;
using Opulenza.Domain.Entities.Products;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Products.Persistence;

public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository
{
    public async Task AddCategoriesToProductWithIdAsync(int productId, IEnumerable<Category> categories,
        CancellationToken cancellationToken)
    {
        await context.Products
            .Where(p => p.Id == productId)
            .ExecuteUpdateAsync(calls => calls.SetProperty(product => product.Categories, categories),
                cancellationToken);
    }

    public async Task<string?> GetLastSlugWithNameAsync(string slug, CancellationToken cancellationToken)
    {
        return await context.Products.Where(p => p.Slug == slug).Select(p => p.Slug)
            .OrderByDescending(s => s).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ProductResult?> GetProductWithIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Products
            .Include(p => p.Images)
            .Include(p => p.Categories)
            .Include(p => p.Ratings)
            .Select(p => new ProductResult
            {
                Id = p.Id,
                Slug = p.Slug,
                Name = p.Name,
                Description = p.Description,
                Brand = p.Brand,
                Tax = p.Tax,
                TaxIncluded = p.TaxIncluded,
                Price = p.Price,
                DiscountPrice = p.DiscountPrice,
                IsAvailable = p.IsAvailable,
                StockQuantity = p.StockQuantity,
                Images = p.Images == null
                    ? null
                    : p.Images.Select(i => new ImageResult
                    {
                        Id = i.Id,
                        FilePath = i.FilePath,
                        IsFeaturedImage = i.IsFeaturedImage,
                    }).ToList(),
                Categories = p.Categories == null
                    ? null
                    : p.Categories.Select(c => new CategoryResult
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        Slug = c.Slug,
                        ParentId = c.ParentId
                    }).ToList(),
                Ratings = p.Ratings == null
                    ? null
                    : p.Ratings.Select(r => new RatingResult
                    {
                        Value = r.Value,
                        UserId = r.UserId,
                        ReviewText = r.ReviewText,
                    }).ToList()
            })
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Products
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<ProductResult?> GetProductWithSlugAsync(string slug, CancellationToken cancellationToken)
    {
        return await context.Products
            .Include(p => p.Images)
            .Include(p => p.Categories)
            .Include(p => p.Ratings)
            .Select(p => new ProductResult
            {
                Id = p.Id,
                Slug = p.Slug,
                Name = p.Name,
                Description = p.Description,
                Brand = p.Brand,
                Tax = p.Tax,
                TaxIncluded = p.TaxIncluded,
                Price = p.Price,
                DiscountPrice = p.DiscountPrice,
                IsAvailable = p.IsAvailable,
                StockQuantity = p.StockQuantity,
                Images = p.Images == null
                    ? null
                    : p.Images.Select(i => new ImageResult
                    {
                        Id = i.Id,
                        FilePath = i.FilePath,
                        IsFeaturedImage = i.IsFeaturedImage,
                    }).ToList(),
                Categories = p.Categories == null
                    ? null
                    : p.Categories.Select(c => new CategoryResult
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        Slug = c.Slug,
                        ParentId = c.ParentId
                    }).ToList(),
                Ratings = p.Ratings == null
                    ? null
                    : p.Ratings.Select(r => new RatingResult
                    {
                        Value = r.Value,
                        UserId = r.UserId,
                        ReviewText = r.ReviewText,
                    }).ToList()
            })
            .FirstOrDefaultAsync(p => p.Slug == slug, cancellationToken);
    }

    public async Task<GetProductListResult> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var filteredProducts = context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            filteredProducts = filteredProducts.Where(p =>
                EF.Functions.Like(p.Name, $"%{query.Search}%") ||
                EF.Functions.Like(p.Description, $"%{query.Search}%"));
        }

        if (!string.IsNullOrWhiteSpace(query.Brand))
        {
            filteredProducts = filteredProducts.Where(p =>
                p.Brand != null && EF.Functions.Like(p.Brand, query.Brand));
        }

        if (query.Category != null)
        {
            filteredProducts = filteredProducts.Where(p =>
                p.Categories != null && p.Categories.Any(c => EF.Functions.Like(p.Name, query.Category)));
        }

        if (query.MinRating.HasValue)
        {
            filteredProducts = filteredProducts.Where(p =>
                p.Ratings != null && p.Ratings.Any() && p.Ratings.Average(r => r.Value) >= query.MinRating);
        }

        if (query.IsAvailable.HasValue)
        {
            filteredProducts = filteredProducts.Where(p => p.IsAvailable == query.IsAvailable);
        }

        if (query.MinPrice.HasValue)
        {
            filteredProducts = filteredProducts.Where(p => p.Price >= query.MinPrice);
        }

        if (query.MaxPrice.HasValue)
        {
            filteredProducts = filteredProducts.Where(p => p.Price <= query.MaxPrice);
        }

        if (query.DiscountOnly == true)
        {
            filteredProducts = filteredProducts.Where(p => p.DiscountPrice != null);
        }

        if (query.SortBy != SortBy.None && query.SortOptions != SortOptions.None)
        {
            filteredProducts = query.SortBy switch
            {
                SortBy.CreatedAt => query.SortOptions == SortOptions.Desc
                    ? filteredProducts.OrderByDescending(p => p.CreatedAt)
                    : filteredProducts.OrderBy(p => p.CreatedAt),
                SortBy.Price => query.SortOptions == SortOptions.Desc
                    ? filteredProducts.OrderByDescending(p => p.Price)
                    : filteredProducts.OrderBy(p => p.Price),
                SortBy.Rating => query.SortOptions == SortOptions.Desc
                    ? filteredProducts.OrderByDescending(p =>
                        p.Ratings != null && p.Ratings.Any() ? p.Ratings.Average(r => r.Value) : 0)
                    : filteredProducts.OrderBy(p =>
                        p.Ratings != null && p.Ratings.Any() ? p.Ratings.Average(r => r.Value) : 0),
                _ => filteredProducts
            };
        }

        var totalCount = await filteredProducts.CountAsync(cancellationToken);

        var results = await filteredProducts
            .Select(p => new GetProductListItemResult()
            {
                Id = p.Id,
                Slug = p.Slug,
                Name = p.Name,
                Description = p.Description,
                Brand = p.Brand,
                Tax = p.Tax,
                TaxIncluded = p.TaxIncluded,
                Price = p.Price,
                DiscountPrice = p.DiscountPrice,
                IsAvailable = p.IsAvailable,
                StockQuantity = p.StockQuantity
            }).Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToListAsync(cancellationToken);


        return new GetProductListResult()
        {
            Products = results,
            TotalCount = totalCount
        };
    }

    public async Task<bool> ExistsAsync(int productId, CancellationToken cancellationToken)
    {
        return await context.Products.AnyAsync(p => p.Id == productId, cancellationToken);
    }
}