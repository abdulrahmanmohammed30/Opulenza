using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator: AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
    }
}