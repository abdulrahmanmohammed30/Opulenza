using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Products.Queries.Common;
using Opulenza.Application.Mapping;
using Opulenza.Domain.Entities.Categories;
using Opulenza.Domain.Entities.Products;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Products.Persistence;

public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository
{
    public async Task AddCategoriesToProductWithId(int productId, IEnumerable<Category> categories,
        CancellationToken cancellationToken)
    {
        await context.Products
            .Where(p => p.Id == productId)
            .ExecuteUpdateAsync(calls => calls.SetProperty(product => product.Categories, categories),
                cancellationToken);
    }


    public async Task<string?> GetLastSlugWithName(string slug, CancellationToken cancellationToken)
    {
        return await context.Products.Where(p => p.Slug == slug).Select(p => p.Slug)
            .OrderByDescending(s => s).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ProductResult?> GetProductWithId(int id, CancellationToken cancellationToken)
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
                Images = p.Images.Select(i => new ImageResult
                {
                    Id = i.Id,
                    FilePath = i.FilePath,
                    IsFeaturedImage = i.IsFeaturedImage,
                }).ToList(),
                Categories = p.Categories.Select(c => new CategoryResult
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Slug = c.Slug,
                    ParentId = c.ParentId
                }).ToList(),
                Ratings = p.Ratings.Select(r => new RatingResult
                {
                    Value = r.Value,
                    UserId = r.UserId,
                    ReviewText = r.ReviewText,
                }).ToList()
            })
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<ProductResult?> GetProductWithSlug(string slug, CancellationToken cancellationToken)
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
                Images = p.Images.Select(i => new ImageResult
                {
                    Id = i.Id,
                    FilePath = i.FilePath,
                    IsFeaturedImage = i.IsFeaturedImage,
                }).ToList(),
                Categories = p.Categories.Select(c => new CategoryResult
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Slug = c.Slug,
                    ParentId = c.ParentId
                }).ToList(),
                Ratings = p.Ratings.Select(r => new RatingResult
                {
                    Value = r.Value,
                    UserId = r.UserId,
                    ReviewText = r.ReviewText,
                }).ToList()
            })
            .FirstOrDefaultAsync(p => p.Slug == slug, cancellationToken);
    }
}