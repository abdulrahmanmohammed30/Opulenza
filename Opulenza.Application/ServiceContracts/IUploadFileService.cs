using Microsoft.AspNetCore.Http;

namespace Opulenza.Application.ServiceContracts;

public interface IUploadFileService
{
    Task<string> UploadFile(IFormFile  file, string directoryPath, CancellationToken cancellationToken = default(CancellationToken));
}