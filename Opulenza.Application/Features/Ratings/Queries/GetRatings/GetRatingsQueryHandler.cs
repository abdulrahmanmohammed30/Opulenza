using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;

namespace Opulenza.Application.Features.Ratings.Queries.GetRatings;

public class GetRatingsQueryHandler(IRatingRepository ratingRepository): IRequestHandler<GetRatingsQuery, ErrorOr<GetRatingsResult>>
{
    public async Task<ErrorOr<GetRatingsResult>> Handle(GetRatingsQuery request, CancellationToken cancellationToken)
    {
        var ratings = await ratingRepository.GetRatingsByProductIdAsync(request, cancellationToken);
        var totalCount = await ratingRepository.CountAsync(request.ProductId, request.Rating, cancellationToken);
        
        return  new GetRatingsResult
        {
            Ratings = ratings,
            TotalCount = totalCount
        };
    }
}