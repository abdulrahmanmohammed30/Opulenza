namespace Opulenza.Contracts.Products;

public class AddCategoriesRequest
{
    public int? ProductId { get; set; }
    public List<int>? Categories { get; set; }
}