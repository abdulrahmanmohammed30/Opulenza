using ErrorOr;
using FluentValidation;

namespace Opulenza.Application.Features.Products.Commands.AddProductImages;

public class AddProductImagesCommandValidator: AbstractValidator<AddProductImagesCommand>
{
    public AddProductImagesCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull()
            .WithMessage("ProductId is required");

        RuleFor(x => x.Files)
            .NotEmpty().WithMessage("Files are required");
    }
    
}