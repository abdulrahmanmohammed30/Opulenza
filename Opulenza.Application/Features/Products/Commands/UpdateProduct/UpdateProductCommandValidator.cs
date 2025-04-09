using FluentValidation;
using Opulenza.Application.Common.interfaces;

namespace Opulenza.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Id is required")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
            // .MustAsync(async (id, cancellationToken) => await productRepository.Exists(id.Value, cancellationToken))
            // .WithMessage("Invalid product id");

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters");

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Description is required");
        
        RuleFor(p => p.Price)
            .NotEmpty().WithMessage("Price is required")
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than 0");

        RuleFor(p => p.DiscountPrice)
            .GreaterThanOrEqualTo(0).WithMessage("DiscountPrice must be greater than 0");

        RuleFor(p => p)
            .Must(p => p.Price >= p.DiscountPrice)
            .When(p => p.Price != null && p.DiscountPrice != null);

        RuleFor(p => p.Tax)
            .NotEmpty().WithMessage("Tax is required")
            .GreaterThanOrEqualTo(0.00m).WithMessage("Tax must be greater than or equal to 0.00%")
            .LessThanOrEqualTo(100.00m).WithMessage("Tax must be less than or equal to 50.00%");
        
        RuleFor(p=>p.TaxIncluded)
            .NotEmpty().WithMessage("TaxIncluded is required");

        RuleFor(p => p.Brand)
            .MaximumLength(80).WithMessage("Brand must not exceed 80 characters");

        RuleFor(p => p.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("StockQuantity must be greater than or equal to 0");

        RuleForEach(p => p.Categories)
            .GreaterThan(0).WithMessage("Category Id must be greater than 0");
    }
}