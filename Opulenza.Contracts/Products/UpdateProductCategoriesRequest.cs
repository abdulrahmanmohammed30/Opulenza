namespace Opulenza.Contracts.Products;

public class UpdateCategoriesRequest
{
    public int? ProductId { get; set; }
    public List<int>? Categories { get; set; }
}