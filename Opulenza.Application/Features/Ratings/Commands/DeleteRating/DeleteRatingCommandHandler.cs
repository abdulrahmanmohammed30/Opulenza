using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Ratings;

namespace Opulenza.Application.Features.Ratings.Commands.DeleteRating;

public class DeleteRatingCommandHandler(
    IRepository<Rating> ratingRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteRatingCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
    {
        var rating = await ratingRepository.GetByIdAsync(request.RatingId.Value, cancellationToken);

        if (rating == null)
        {
            return Error.NotFound("RatingNotFound", $"Rating with id {request.RatingId} not found.");
        }

        rating.IsDeleted = true;

        ratingRepository.Update(rating);
        await unitOfWork.CommitChangesAsync(cancellationToken);

        return "Rating deleted successfully.";
    }
}