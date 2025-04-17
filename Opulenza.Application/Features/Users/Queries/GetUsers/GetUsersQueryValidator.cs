using FluentValidation;
using Opulenza.Application.Features.Common;

namespace Opulenza.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryValidator:AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        
        Include(new PaginatedQueryValidator());
        
        RuleFor(p => p.Email)
            .EmailAddress().WithMessage("Invalid email address");
    }
}