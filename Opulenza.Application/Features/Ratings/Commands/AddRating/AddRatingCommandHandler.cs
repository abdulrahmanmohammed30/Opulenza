using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Ratings;
using Serilog.Context;

namespace Opulenza.Application.Features.Ratings.Commands.AddRating;

public class AddRatingCommandHandler(
    IProductRepository productRepository,
    IUserRepository userRepository,
    ICurrentUserProvider currentUserProvider,
    IRatingRepository ratingRepository,
    ILogger<AddRatingCommandHandler> logger,
    IUnitOfWork unitOfWork) : IRequestHandler<AddRatingCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(AddRatingCommand request, CancellationToken cancellationToken)
    {
        var doesProductExist = await productRepository.ExistsAsync(request.ProductId!.Value, cancellationToken);
        if (!doesProductExist)
        {
            return Error.NotFound("ProductNotFound", $"Product with id {request.ProductId} not found.");
        }

        var userId = currentUserProvider.GetCurrentUser().Id;

        var doesUserExist = await userRepository.ExistsAsync(userId, cancellationToken);
        if (!doesUserExist)
        {
            return Error.NotFound("UserNotFound", $"User with id {userId} not found.");
        }

        var doesRatingExist = await ratingRepository.ExistsAsync(userId, request.ProductId.Value, cancellationToken);
        if (doesRatingExist)
        {
            using (LogContext.PushProperty("UserId", userId))
            using (LogContext.PushProperty("ProductId", request.ProductId.Value))
            {
                logger.LogWarning("Duplicate user rating");
            }

            return Error.Conflict("DuplicateUserRating", "User has already rated this product.");
        }

        var rating = new Rating
        {
            Value = request.Value!.Value,
            ProductId = request.ProductId.Value,
            UserId = userId,
            ReviewText = request.ReviewText
        };

        ratingRepository.Add(rating);
        await unitOfWork.CommitChangesAsync(cancellationToken);

        return rating.Id;
    }
}