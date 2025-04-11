namespace Opulenza.Contracts.Categories;

public class AddCategoryRequest
{
    public string? Name { get; set; } 
    
    public string? Description { get; set; } 
    
    public int? ParentId { get; set; }
}