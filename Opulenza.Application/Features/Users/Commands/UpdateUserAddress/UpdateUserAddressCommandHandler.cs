using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Users.Commands.UpdateUserAddress;

public class UpdateUserAddressCommandHandler(ICurrentUserProvider currentUserProvider, UserManager<ApplicationUser> userManager): IRequestHandler<UpdateUserAddressCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(UpdateUserAddressCommand request, CancellationToken cancellationToken)
    {
        var username = currentUserProvider.GetCurrentUser().Username;
        
        if (string.IsNullOrEmpty(username))
        {
            return Error.Unauthorized();
        }
        
        var user = await userManager.Users.Include(u=>u.Address).FirstOrDefaultAsync(u => u.UserName == username);

        if (user == null)
        {
            return Error.Unauthorized();
        }
        
        user.Address = new UserAddress
        {
            Id= user.Address?.Id ?? 0,
            StreetAddress = request.StreetAddress,
            City = request.City,
            ZipCode = request.ZipCode,
            Country = request.Country
        };

        await userManager.UpdateAsync(user);
        
        return "";
    }
}