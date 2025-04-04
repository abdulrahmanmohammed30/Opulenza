using FluentValidation;

namespace Opulenza.Application.Features.Users.Commands.UpdateUserAddress;

public class UpdateUserAddressCommandValidator: AbstractValidator<UpdateUserAddressCommand>
{
   public UpdateUserAddressCommandValidator()
   {
      RuleFor(x => x.StreetAddress)
         .NotEmpty().WithMessage("Address is required.")
         .MinimumLength(10).WithMessage("Address must be at least 10 characters long.")
         .MaximumLength(255).WithMessage("Address must be at most 255 characters long.");
      
      RuleFor(x => x.Country)
         .NotEmpty()
         .WithMessage("Country is required.")
         .MinimumLength(2)
         .WithMessage("Country must be at least 2 characters long.")
         .MaximumLength(100)
         .WithMessage("Country must be at most 100 characters long.");

      RuleFor(x => x.City)
         .NotEmpty().WithMessage("City is required.")
         .MinimumLength(2).WithMessage("City must be at least 2 characters long.")
         .MaximumLength(100).WithMessage("City must be at most 100 characters long.");
      
      RuleFor(x => x.ZipCode)
         .NotEmpty()
         .WithMessage("Zip code is required.")
         .Matches(@"^\d{5}(-\d{4})?$")
         .WithMessage("Zip code must be in the format 12345 or 12345-6789.");
   }   
}