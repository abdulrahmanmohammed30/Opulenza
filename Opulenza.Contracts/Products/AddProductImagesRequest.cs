using Microsoft.AspNetCore.Http;

namespace Opulenza.Contracts.Products;

public class AddProductImagesRequest
{
    public List<IFormFile>? Files { get; set; }
}