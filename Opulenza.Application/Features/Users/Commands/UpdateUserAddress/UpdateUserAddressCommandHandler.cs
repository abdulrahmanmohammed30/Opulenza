using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Users;
using Serilog.Context;

namespace Opulenza.Application.Features.Users.Commands.UpdateUserAddress;

public class UpdateUserAddressCommandHandler(ICurrentUserProvider currentUserProvider, 
    UserManager<ApplicationUser> userManager, ILogger<UpdateUserAddressCommandHandler> logger): IRequestHandler<UpdateUserAddressCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(UpdateUserAddressCommand request, CancellationToken cancellationToken)
    {
        var username = currentUserProvider.GetCurrentUser().Username;
        
        if (string.IsNullOrEmpty(username))
        {
            logger.LogWarning("Username is null or empty");
            return Error.Unauthorized();
        }
        
        var user = await userManager.Users.Include(u=>u.Address).FirstOrDefaultAsync(u => u.UserName == username);

        if (user == null)
        {
            logger.LogWarning("User with username {Username} not found", username);
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
        
        return "Address updated successfully";
    }
}