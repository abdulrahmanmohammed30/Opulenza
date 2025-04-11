using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Mapping;

namespace Opulenza.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetCategoriesQuery, ErrorOr<GetCategoriesResult>>
{
    public async Task<ErrorOr<GetCategoriesResult>> Handle(GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetCategoriesAsync(request, cancellationToken);
        var totalCount = await categoryRepository.GetCategoriesCountAsync(request, cancellationToken);
        
        var result = new GetCategoriesResult
        {
            Categories = categories.Select(c=>c.MapToGetCategoryResult()).ToList(),
            TotalCount = totalCount
        };
        return result;
    }
}