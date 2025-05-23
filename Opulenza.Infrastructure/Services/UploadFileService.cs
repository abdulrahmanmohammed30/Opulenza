﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Opulenza.Application.ServiceContracts;
using Opulenza.Infrastructure.Settings;

namespace Opulenza.Infrastructure.Services;

public class UploadFileService(IWebHostEnvironment webHostEnvironment, IOptions<FileSettings> fileSettingsOptions)
    : IUploadFileService
{
    private FileSettings FileSettings => fileSettingsOptions.Value;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="directoryPath">The director path relative to the wwwroot, ex. products/productId/images</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<string> UploadFile(IFormFile file, string directoryPath,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        directoryPath = directoryPath.Replace('/', '\\');
        // validation 
        if (file is null || file.Length == 0)
        {
            throw new ArgumentException("file is empty", nameof(file));
        }

        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            throw new ArgumentException("directory path is empty", nameof(directoryPath));
        }

        if (FileSettings.MaxFileSize < file.Length)
        {
            throw new ArgumentException($"file size is larger than {FileSettings.MaxFileSize}", nameof(file));
        }

        var fileExtension = Path.GetExtension(file.FileName);

        if (!FileSettings.AllowedExtensions.Contains(fileExtension.ToLowerInvariant()))
        {
            throw new ArgumentException($"file extension is not allowed", nameof(file));
        }

        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
        var uniqueFileName = $"{Guid.NewGuid().ToString().Replace("-", string.Empty)}{fileExtension}";
        var relativeFilePath = Path.Combine(directoryPath, uniqueFileName);
        var physicalDirectoryPath = Path.Combine(webHostEnvironment.WebRootPath, directoryPath);
        var physicalFilePath = Path.Combine(webHostEnvironment.WebRootPath, relativeFilePath);

        if (Directory.Exists(physicalDirectoryPath) == false)
            Directory.CreateDirectory(physicalDirectoryPath);

        await using var fileStream =
            new FileStream(physicalFilePath, FileMode.Create);
        await file.CopyToAsync(fileStream, cancellationToken);
        return relativeFilePath;
    }
}