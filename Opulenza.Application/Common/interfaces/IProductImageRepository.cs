using Opulenza.Domain.Entities.Products;

namespace Opulenza.Application.Common.interfaces;

public interface IProductImageRepository: IRepository<ProductImage>
{
    void AddImages(IEnumerable<ProductImage> productImages);

    Task<List<ProductImage>> GetImagesByProductId(int productId, CancellationToken cancellationToken);
}