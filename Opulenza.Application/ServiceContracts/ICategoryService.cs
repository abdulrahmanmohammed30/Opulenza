namespace Opulenza.Application.ServiceContracts;

public interface ICategoryService
{
    Task<string> GenerateUniqueSlugAsync(string categoryName, CancellationToken cancellationToken);
}