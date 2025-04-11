using Opulenza.Application.Features.Common;

namespace Opulenza.Application.Features.ProductCategories.Queries.GetProductCategories;

public class GetProductCategoriesResult
{
    public List<CategoryResult> Categories { get; set; } = new ();
}