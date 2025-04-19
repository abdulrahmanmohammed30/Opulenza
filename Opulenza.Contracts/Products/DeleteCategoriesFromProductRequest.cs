using Microsoft.AspNetCore.Mvc;

namespace Opulenza.Contracts.Products;

public class DeleteCategoriesFromProductRequest
{
    public List<int>? Categories { get; init; }
}