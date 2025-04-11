using FluentValidation;
using Opulenza.Application.Features.Products.Commands.AddProductImages;

namespace Opulenza.Application.Features.Categories.Commands.AddCategoryImages;

public class AddCategoryImagesCommandValidator: AbstractValidator<AddProductImagesCommand>
{
    public AddCategoryImagesCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull()
            .WithMessage("ProductId is required");

        RuleFor(x => x.Files)
            .NotEmpty().WithMessage("Files are required");
    }
    
}