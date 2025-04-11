using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand: IRequest<ErrorOr<string>>
{
    public int? Id { get; set; }
    
    public string? Name { get; set; } 
    
    public string? Description { get; set; } 
    
    public int? ParentId { get; set; }
}