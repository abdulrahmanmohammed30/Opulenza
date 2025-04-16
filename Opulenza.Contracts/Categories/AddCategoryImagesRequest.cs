using Microsoft.AspNetCore.Http;

namespace Opulenza.Contracts.Categories;

public class AddCategoryImagesRequest
{
    public List<IFormFile>? Files { get; set; }
}