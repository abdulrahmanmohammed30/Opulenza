using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Categories.Commands.AddCategory;

public class AddCategoryCommand: IRequest<ErrorOr<int>>
{
    public string? Name { get; set; } 
    
    public string? Description { get; set; } 
    
    public int? ParentId { get; set; }
}