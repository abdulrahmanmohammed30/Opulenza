namespace Opulenza.Contracts.Products;

public class DeleteCategoriesRequest
{
    public int? ProductId { get; init; }
    public List<int>? Categories { get; init; }
}