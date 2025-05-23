﻿using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.ProductCategories.Commands.UpdateProductCategories;

public class UpdateProductCategoriesCommand: IRequest<ErrorOr<string>>
{
    public int? ProductId { get; set; }
    public List<int>? Categories { get; set; }
}