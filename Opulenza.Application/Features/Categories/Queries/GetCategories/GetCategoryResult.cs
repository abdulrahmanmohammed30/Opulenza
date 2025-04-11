namespace Opulenza.Application.Features.Categories.Queries.GetCategories;

public class GetCategoryResult
{
    public int Id { get; set; }
    public required string Name { get; set; } 
    
    public required string Description { get; set; } 

    public required string Slug { get; set; } 

    public int? ParentId { get; set; }
}