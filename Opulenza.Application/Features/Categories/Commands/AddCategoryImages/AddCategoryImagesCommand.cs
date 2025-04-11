using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Opulenza.Application.Features.Categories.Commands.AddCategoryImages;

public class AddCategoryImagesCommand: IRequest<ErrorOr<CategoryImagesResult>>
{
    public int? CategoryId { get; set; }
    
    public List<IFormFile>? Files { get; set; }
}