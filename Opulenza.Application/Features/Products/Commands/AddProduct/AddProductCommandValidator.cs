using FluentValidation;

namespace Opulenza.Application.Features.Products.Commands.AddProduct;

public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    public AddProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters");

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(p => p.Price)
            .NotEmpty().WithMessage("Price is required")
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than 0");

        RuleFor(p => p.DiscountPrice)
            // .NotEmpty().WithMessage("Discount price is required")
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than 0")
            .LessThanOrEqualTo(p => p.Price);
        
        RuleFor(p => p.Tax)
            // .NotEmpty().WithMessage("Tax is required")
            .GreaterThanOrEqualTo(0.00m).WithMessage("Tax must be greater than or equal to 0.00%")
            .LessThanOrEqualTo(100.00m).WithMessage("Tax must be less than or equal to 50.00%");
        
        RuleFor(p=>p.TaxIncluded)
            .NotEmpty().WithMessage("TaxIncluded is required");
        
        RuleFor(p => p.Brand)
            .MaximumLength(80).WithMessage("Brand must not exceed 80 characters");

        RuleFor(p => p.StockQuantity)
            .NotEmpty().WithMessage("StockQuantity is required")
            .GreaterThanOrEqualTo(0).WithMessage("StockQuantity must be greater than or equal to 0");

        // RuleFor(p => p.IsAvailable)
        //     .NotEmpty().WithMessage("IsAvailable is required");
        //
        // RuleFor(p=>p.IsAvailable)
        //     .Equal(true).When(p=>p.StockQuantity is not null && p.StockQuantity > 0).WithMessage("IsAvailable must be true when StockQuantity is greater than 0")
        //     .Equal(false).When(p=>p.StockQuantity is null or < 0).WithMessage("IsAvailable must be false when StockQuantity is null or less than 0");
      
        RuleForEach(p => p.Categories)
            .GreaterThan(0).WithMessage("Category Id must be greater than 0");
    }
}
