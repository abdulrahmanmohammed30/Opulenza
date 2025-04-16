using FluentValidation;

namespace Opulenza.Application.Features.Products.Commands.DeleteProductImage;

public class DeleteProductImageCommandValidator: AbstractValidator<DeleteProductImageCommand>
{
    public DeleteProductImageCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product Id is required.")
            .GreaterThan(0).WithMessage("Product Id must be greater than 0.");

        RuleFor(x => x.ImageId)
            .NotEmpty().WithMessage("Image Id is required.")
            .GreaterThan(0).WithMessage("Image Id must be greater than 0.");
    }
}