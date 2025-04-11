namespace Opulenza.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesResult
{
    public List <GetCategoryResult> Categories { get; set; } = new();
    public int TotalCount { get; set; }
}