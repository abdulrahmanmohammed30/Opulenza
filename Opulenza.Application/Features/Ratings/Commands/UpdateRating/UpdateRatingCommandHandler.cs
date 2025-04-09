using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Ratings;

namespace Opulenza.Application.Features.Ratings.Commands.UpdateRating;

public class UpdateRatingCommandHandler(
    IRepository<Rating> ratingRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateRatingCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
    {
        var rating = await ratingRepository.GetByIdAsync(request.RatingId.Value, cancellationToken);

        if (rating == null)
        {
            return Error.NotFound("RatingNotFound", $"Rating with id {request.RatingId} not found.");
        }

        rating.ReviewText = request.ReviewText;
        rating.Value = request.Value.Value;

        ratingRepository.Update(rating);
        await unitOfWork.CommitChangesAsync(cancellationToken);

        return  "Rating updated successfully.";
    }
}