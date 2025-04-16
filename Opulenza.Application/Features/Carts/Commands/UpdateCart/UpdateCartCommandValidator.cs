using FluentValidation;

namespace Opulenza.Application.Features.Carts.Commands.UpdateCart;

public class UpdateCartCommandValidator: AbstractValidator<UpdateCartCommand>
{
    public UpdateCartCommandValidator()
    {
        RuleForEach(x => x.Items)
            .ChildRules(items =>
            {
                items.RuleFor(x => x.ProductId)
                    .NotEmpty()
                    .WithMessage("Product ID cannot be empty.");
                
                items.RuleFor(x => x.Quantity)
                    .NotNull()
                    .WithMessage("Quantity cannot be null.")
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Quantity must be greater than or equal 0.");
            });

        RuleFor(x => x)
            .Must((x, _) => BeUnique(x.Items));

    }

    private static bool BeUnique(List<UpdateCartItemCommand> items)
    {
        if (items.Count is 0 or 1)
        {
            return true;
        }
        
        
        var orderedItems = items.OrderBy(x=>x.ProductId).ToList();
        var count = orderedItems.Count;
        for (var i = 1; i < count; i++)
        {
            if(orderedItems[i].ProductId == orderedItems[i-1].ProductId)
            {
                return false;
            }
        }

        return true;
    }
}