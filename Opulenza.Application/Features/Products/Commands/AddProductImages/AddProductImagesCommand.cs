using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Opulenza.Application.Features.Products.Commands.AddProductImages;

public class AddProductImagesCommand: IRequest<ErrorOr<ProductImagesResult>>
{
    public int? ProductId { get; set; }
    
    public List<IFormFile>? Files { get; set; }
}