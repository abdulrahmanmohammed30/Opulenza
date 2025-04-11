using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Helpers;
using Opulenza.Application.ServiceContracts;

namespace Opulenza.Application.Services;

public class CategoryService(ICategoryRepository categoryRepository): ICategoryService
{
    public async Task<string> GenerateUniqueSlugAsync(string categoryName, CancellationToken cancellationToken)
    {
        var slug = SlugHelper.GenerateSlug(categoryName);

        var lastSlug = await categoryRepository.GetLastSlugWithNameAsync(slug, cancellationToken);

        if (lastSlug == null)
            return slug;

        var slugNumber = lastSlug.Replace(lastSlug, "");

        if (string.IsNullOrWhiteSpace(slugNumber))
            return $"{slug}-1";

        return $"{slug}-{int.Parse(slugNumber.Replace("-", "")) + 1}";
    }
}

