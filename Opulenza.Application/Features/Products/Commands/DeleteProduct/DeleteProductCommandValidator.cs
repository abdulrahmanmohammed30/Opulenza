using FluentValidation;

namespace Opulenza.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandValidator: AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.")
            .GreaterThanOrEqualTo(1).WithMessage("ProductId must be greater than or equal to 1.");
    }
    
}