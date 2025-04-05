using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Carts;
using Opulenza.Domain.Entities.Ratings;
using Opulenza.Domain.Entities.Users;
using Opulenza.Domain.Entities.Wishlists;

namespace Opulenza.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(
    UserManager<ApplicationUser> userManager,
    ICurrentUserProvider currentUserProvider,
    ISoftDeleteRepository<Cart> cartSoftDeleteRepository,
    ISoftDeleteRepository<Rating> ratingSoftDeleteRepository,
    ISoftDeleteRepository<UserAddress> userAddressSoftDeleteRepository,
    ISoftDeleteRepository<UserImage> userImageSoftDeleteRepository,
    ISoftDeleteRepository<WishListItem> wishListItemSoftDeleteRepository,
    IUnitOfWork unitOfWork, ILogger<DeleteUserCommandHandler> logger) : IRequestHandler<DeleteUserCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var username = currentUserProvider.GetCurrentUser().Username;
        var user = await userManager.FindByNameAsync(username);

        if (user == null)
        {
            logger.LogWarning("User with username {Username} not found", username);
            return Error.Unauthorized();
        }

        try
        {
            user.IsDeleted = true;

            await unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await userManager.UpdateAsync(user);

                // I did not use IUnitOfWord, because I use ExecuteUpdate that sends the SQL UPDATE command directly to the database 
                await cartSoftDeleteRepository.SoftDeleteByUserIdAsync(user.Id);
                await ratingSoftDeleteRepository.SoftDeleteByUserIdAsync(user.Id);
                await userAddressSoftDeleteRepository.SoftDeleteByUserIdAsync(user.Id);
                await userImageSoftDeleteRepository.SoftDeleteByUserIdAsync(user.Id);
                await wishListItemSoftDeleteRepository.SoftDeleteByUserIdAsync(user.Id);

                // for the user order, If the delete was permanent, the userId gets set to null but currently 
                // since I cannot soft delete the order because the orders should not be deleted when users gets deleted 
                // I also cannot set the userId to null, since I undo this so I userId on the order without doing any changes 
            }, cancellationToken);
            return "Account deleted successfully";
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Failed to delete user {Username}", username);
            return "Failed to delete user";
        }
    }
}