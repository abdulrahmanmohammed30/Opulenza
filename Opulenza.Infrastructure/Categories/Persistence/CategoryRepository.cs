using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Categories.Queries.GetCategories;
using Opulenza.Domain.Entities.Categories;
using Opulenza.Infrastructure.Common.Persistence;

namespace Opulenza.Infrastructure.Categories.Persistence;

public class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository
{
    public async Task<List<Category>> GetCategoriesAsync(IEnumerable<int> categoryIds, CancellationToken cancellationToken = default)
    {
        return await context.Categories.Where(c => categoryIds.Contains(c.Id)).ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await context.Categories.AnyAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Categories.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<string?> GetLastSlugWithNameAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await context.Categories
            .Where(c => c.Slug == slug)
            .Select(x => x.Slug)
            .OrderByDescending(x => x)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task DeleteImagesAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        await context.Categories.Where(c => c.Id == categoryId)
            .ExecuteUpdateAsync(calls => calls.SetProperty(x => x.IsDeleted, true), cancellationToken);
    }
    
    public async Task<List<Category>> GetCategoriesAsync(GetCategoriesQuery query,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Category> categoriesQuery = context.Categories;

        if (query.Search != null)
        {
            categoriesQuery = categoriesQuery
                .Where(c => EF.Functions.Like(c.Name, $"%{query.Search}%"));
        }
        
        if (query.Sort != null && query.Sort == true)
        {
            categoriesQuery = categoriesQuery.OrderBy(c=>c.Name);
        }

        categoriesQuery=categoriesQuery.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);
        
        return await categoriesQuery.ToListAsync(cancellationToken);
    }
    
    public async Task<int> GetCategoriesCountAsync(GetCategoriesQuery query,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Category> categoriesQuery = context.Categories;

        if (query.Search != null)
        {
            categoriesQuery = categoriesQuery
                .Where(c => EF.Functions.Like(c.Name, $"%{query.Search}%"));
        }
        
        if (query.Sort != null && query.Sort == true)
        {
            categoriesQuery = categoriesQuery.OrderBy(c=>c.Name);
        }
        
        return await categoriesQuery.CountAsync(cancellationToken);
    }

    public async Task<bool> HasAncestorCategory(int categoryId, int parentId,
        CancellationToken cancellationToken = default)
    {
        var list = await context.CategoryRelationships
            .FromSql($"EXECUTE dbo.usp_GetCategoryRelationship @parentCategoryId={categoryId}, @categoryId={parentId}")
            .ToListAsync(cancellationToken);

        return list.Any() == false;
    }

}